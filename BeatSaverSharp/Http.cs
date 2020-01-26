using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BeatSaverSharp.Exceptions;

namespace BeatSaverSharp
{
    /// <summary>
    /// HTTP Options
    /// </summary>
    public struct HttpOptions
    {
        /// <summary>
        /// Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Application Version
        /// </summary>
        public Version Version { get; set; }

        /// <summary>
        /// Optional HTTP Timeout Override
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// Additional agents to list in User-Agent string
        /// </summary>
        public ApplicationAgent[] Agents { get; set; }
    }

    /// <summary>
    /// Agent Information
    /// </summary>
    public struct ApplicationAgent
    {
        /// <summary>
        /// Agent Name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Agent Version String
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Application Agent
        /// </summary>
        /// <param name="name">Agent Name</param>
        /// <param name="version">Agent Version</param>
        public ApplicationAgent(string name, Version version)
        {
            Name = name;
            Version = version.ToString();
        }

        /// <summary>
        /// Application Agent
        /// </summary>
        /// <param name="name">Agent Name</param>
        /// <param name="version">Agent Version String</param>
        public ApplicationAgent(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }

    internal sealed class Http
    {
        internal static readonly JsonSerializer Serializer = new JsonSerializer();

        internal static readonly Http Default = new Http(new HttpOptions()
        {
            ApplicationName = null,
            Version = null,
            Timeout = TimeSpan.FromSeconds(30),
        });

        internal HttpClient Client { get; private set; }

        internal Http(HttpOptions options = new HttpOptions())
        {
            if ((options.ApplicationName != null && options.Version == null) || (options.ApplicationName == null && options.Version != null))
            {
                throw new ArgumentException("You must specify either both or none of ApplicationName and Version");
            }

            Client = new HttpClient()
            {
                BaseAddress = new Uri($"{BeatSaver.BaseURL}/api/"),
                Timeout = options.Timeout ?? TimeSpan.FromSeconds(30),
            };

            string libVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string userAgent = $"BeatSaverSharp/{libVersion}";

            if (options.ApplicationName != null)
            {
                userAgent = $"{options.ApplicationName}/{options.Version} {userAgent}";
            }

            foreach (var agent in options.Agents ?? new ApplicationAgent[0])
            {
                if (agent.Name == null || agent.Version == null)
                {
                    throw new ArgumentException("All application agents must specify both name and version");
                }

                userAgent += $" {agent.Name}/{agent.Version.ToString()}";
            }

            Client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }

        internal async Task<HttpResponse> GetAsync(string url, CancellationToken token, IProgress<double> progress = null)
        {
            HttpResponseMessage resp = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
            if ((int)resp.StatusCode == 429)
            {
                throw new RateLimitExceededException(resp);
            }

            if (token.IsCancellationRequested) throw new TaskCanceledException();

            using (MemoryStream ms = new MemoryStream())
            using (Stream s = await resp.Content.ReadAsStreamAsync())
            {
                byte[] buffer = new byte[1 << 13];
                int bytesRead;

                long? contentLength = resp.Content.Headers.ContentLength;
                long totalRead = 0;
                progress?.Report(0);

                while ((bytesRead = await s.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    if (token.IsCancellationRequested) throw new TaskCanceledException();

                    if (contentLength != null)
                    {
                        double prog = (double)totalRead / (double)contentLength;
                        progress?.Report(prog);
                    }

                    await ms.WriteAsync(buffer, 0, bytesRead);
                    totalRead += bytesRead;
                }

                progress?.Report(1);
                byte[] bytes = ms.ToArray();

                return new HttpResponse(resp, bytes);
            }
        }
    }

    /// <summary>
    /// Rate Limit Info
    /// </summary>
    public struct RateLimitInfo
    {
        /// <summary>
        /// Number of requests remaining
        /// </summary>
        public readonly int Remaining;
        /// <summary>
        /// Rate Limit Reset Time
        /// </summary>
        public readonly DateTime Reset;
        /// <summary>
        /// Total allowed requests for a given window
        /// </summary>
        public readonly int Total;

        /// <summary>
        /// </summary>
        /// <param name="remaining"></param>
        /// <param name="reset"></param>
        /// <param name="total"></param>
        public RateLimitInfo(int remaining, DateTime reset, int total)
        {
            Remaining = remaining;
            Reset = reset;
            Total = total;
        }

        internal static RateLimitInfo? FromHttp(HttpResponseMessage resp)
        {
            int _remaining;
            DateTime? _reset = null;
            int _total;

            if (resp.Headers.TryGetValues("Rate-Limit-Remaining", out IEnumerable<string> remainings))
            {
                string val = remainings.FirstOrDefault();
                if (val != null)
                {
                    if (int.TryParse(val, out int result)) _remaining = result;
                    else return null;
                }
                else return null;
            }
            else return null;

            if (resp.Headers.TryGetValues("Rate-Limit-Total", out IEnumerable<string> totals))
            {
                string val = totals.FirstOrDefault();
                if (val != null)
                {
                    if (int.TryParse(val, out int result)) _total = result;
                    else return null;
                }
                else return null;
            }
            else return null;

            if (resp.Headers.TryGetValues("Rate-Limit-Reset", out IEnumerable<string> resets))
            {
                string val = resets.FirstOrDefault();
                if (val != null)
                {
                    if (ulong.TryParse(val, out ulong ts))
                    {
                        _reset = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                        _reset = _reset?.AddSeconds(ts).ToLocalTime();
                    }
                    else return null;
                }
            }
            else return null;

            if (_reset == null) return null;
            return new RateLimitInfo(_remaining, (DateTime)_reset, _total);
        }
    }

    internal class HttpResponse
    {
        public readonly HttpStatusCode StatusCode;
        public readonly string ReasonPhrase;
        public readonly HttpResponseHeaders Headers;
        public readonly HttpRequestMessage RequestMessage;
        public readonly bool IsSuccessStatusCode;
        public readonly RateLimitInfo? RateLimit;

        private readonly byte[] _body;

        internal HttpResponse(HttpResponseMessage resp, byte[] body)
        {
            StatusCode = resp.StatusCode;
            ReasonPhrase = resp.ReasonPhrase;
            Headers = resp.Headers;
            RequestMessage = resp.RequestMessage;
            IsSuccessStatusCode = resp.IsSuccessStatusCode;
            RateLimit = RateLimitInfo.FromHttp(resp);

            _body = body;
        }

        public byte[] Bytes() => _body;
        public string String() => Encoding.UTF8.GetString(_body);
        public T JSON<T>()
        {
            string body = String();

            using (StringReader sr = new StringReader(body))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                return Http.Serializer.Deserialize<T>(reader);
            }
        }
    }
}
