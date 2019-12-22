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
    internal class Http
    {
        private static bool headersInit = false;
        internal static HttpClient Client = new HttpClient()
        {
            BaseAddress = new Uri($"{BeatSaver.BaseURL}/api/"),
            Timeout = TimeSpan.FromSeconds(30),
        };

        internal static JsonSerializer Serializer = new JsonSerializer();

        private static void InitHeaders()
        {
            if (headersInit == true) return;
            headersInit = true;

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Client.DefaultRequestHeaders.Add("User-Agent", $"BeatSaver.Net/{version}");
        }
        internal static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        internal static readonly string BaseUserAgent = $"{0}/{1} (BeatSaver.Net/{Version})";
        internal static readonly Uri BeatSaverBaseUri = new Uri(BeatSaver.BaseURL);

        internal static async Task<HttpResponse> GetAsync(string url, RequestorInfo requestorInfo, CancellationToken token, IProgress<double> progress = null)
        {
            //InitHeaders();
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(BeatSaverBaseUri, url),
                Method = HttpMethod.Get
            };
            request.Headers.Add("User-Agent", requestorInfo.UserAgent);
            HttpResponseMessage resp = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token).ConfigureAwait(false);
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

                return new HttpResponse(resp, requestorInfo, bytes);
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
        internal readonly RequestorInfo RequestorInfo;
        private readonly byte[] _body;

        internal HttpResponse(HttpResponseMessage resp, RequestorInfo requestorInfo, byte[] body)
        {
            StatusCode = resp.StatusCode;
            ReasonPhrase = resp.ReasonPhrase;
            Headers = resp.Headers;
            RequestMessage = resp.RequestMessage;
            IsSuccessStatusCode = resp.IsSuccessStatusCode;
            RequestorInfo = requestorInfo;
            RateLimit = RateLimitInfo.FromHttp(resp);

            _body = body;
        }

        public byte[] Bytes() => _body;
        public string String() => Encoding.UTF8.GetString(_body);
        public T JSON<T>()
            where T : Types.IHasRequestor
        {
            string body = String();

            using (StringReader sr = new StringReader(body))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                T retObj = Http.Serializer.Deserialize<T>(reader);
                if (retObj is Types.IHasRequestor withRequestor)
                    retObj.RequestorInfo = RequestorInfo;
                return retObj;
            }
        }
    }
}
