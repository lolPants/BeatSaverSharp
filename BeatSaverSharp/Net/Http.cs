using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BeatSaverSharp.Exceptions;
using Newtonsoft.Json;

namespace BeatSaverSharp.Net
{
    internal sealed class Http : IDisposable
    {
        internal static readonly JsonSerializer Serializer = new();

        internal bool Disposed { get; private set; }
        internal HttpOptions Options { get; }
        internal HttpClient Client { get; }

        private readonly ConcurrentDictionary<string, string> _etagCache = new();
        private readonly ConcurrentDictionary<string, HttpResponse> _responseCache = new();

        internal Http(HttpOptions options)
        {
            Options = options;

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };

            Client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"{Options.BaseURL}/api/"),
                Timeout = Options.Timeout,
            };

            Client.DefaultRequestHeaders.Add("User-Agent", options.UserAgent);
        }

        internal async Task<HttpResponse> GetAsync(HttpRequest request)
        {
            if (Disposed == true)
            {
                throw new ObjectDisposedException(nameof(Http));
            }

            var token = request.Token ?? CancellationToken.None;
            var msg = new HttpRequestMessage(HttpMethod.Get, request.Uri)
            {
                Version = Options.HttpVersion,
            };

            foreach (var header in request.Headers)
            {
                if (header.Key.ToLower() == "User-Agent".ToLower()) continue;
                if (header.Key.ToLower() == "If-None-Match".ToLower()) continue;

                msg.Headers.Add(header.Key, header.Value);
            }

            string? etagMatch = null;
            if (Options.DisableCaching == false)
            {
                if (_etagCache.TryGetValue(request.Uri, out etagMatch))
                {
                    if (_responseCache.ContainsKey(etagMatch))
                    {
                        msg.Headers.Add("If-None-Match", etagMatch);
                    }
                }
            }

            var resp = await Client
                .SendAsync(msg, HttpCompletionOption.ResponseHeadersRead, token)
                .ConfigureAwait(false);

            if ((int)resp.StatusCode == 429)
            {
                var ex = new RateLimitExceededException(resp);
                if (Options.HandleRateLimits == false)
                {
                    throw ex;
                }

                RateLimitInfo info = ex.RateLimit;
                TimeSpan diff = info.Reset - DateTime.Now;

                int millis = (int)diff.TotalMilliseconds;
                if (millis > 0)
                {
                    await Task.Delay(millis).ConfigureAwait(false);
                }

                return await GetAsync(request).ConfigureAwait(false);
            }

            if (Options.DisableCaching == false && etagMatch is not null && resp.StatusCode == HttpStatusCode.NotModified)
            {
                if (_responseCache.TryGetValue(etagMatch, out var cached))
                {
                    return cached;
                }
            }

            if (token.IsCancellationRequested)
            {
                throw new TaskCanceledException();
            }

            using MemoryStream ms = new MemoryStream();
            using Stream s = await resp.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var bufferSize = 1 << 13;
            int bytesRead;

#if NETSTANDARD2_1
            var buffer = new Memory<byte>(new byte[bufferSize]);
#else
            var buffer = new byte[bufferSize];
#endif

            long? contentLength = resp.Content.Headers.ContentLength;
            long totalRead = 0;
            request.Progress?.Report(0);

#if NETSTANDARD2_1
            while ((bytesRead = await s.ReadAsync(buffer, token).ConfigureAwait(false)) > 0)
#else
            while ((bytesRead = await s.ReadAsync(buffer, 0, buffer.Length, token).ConfigureAwait(false)) > 0)
#endif
            {
                if (token.IsCancellationRequested) throw new TaskCanceledException();
                if (contentLength is not null)
                {
                    double prog = (double)totalRead / (double)contentLength;
                    request.Progress?.Report(prog);
                }

#if NETSTANDARD2_1
                await ms.WriteAsync(buffer.Slice(0, bytesRead), token).ConfigureAwait(false);
#else
                await ms.WriteAsync(buffer, 0, bytesRead, token).ConfigureAwait(false);
#endif

                totalRead += bytesRead;
            }

            request.Progress?.Report(1);
            byte[] bytes = ms.ToArray();

            var response = new HttpResponse(resp, bytes);
            if (Options.DisableCaching == false && resp.Headers.TryGetValues("ETag", out System.Collections.Generic.IEnumerable<string> values))
            {
                var etag = values.FirstOrDefault();
                if (etag is not null)
                {
                    _etagCache[request.Uri] = etag;
                    _responseCache[etag] = response;
                }
            }

            return response;
        }

        public void Dispose()
        {
            if (Disposed == false)
            {
                Disposed = true;
                Client.Dispose();
            }
        }
    }
}
