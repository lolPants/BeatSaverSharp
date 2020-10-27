using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    internal sealed class HttpResponse
    {
        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; }
        public HttpResponseHeaders Headers { get; }
        public HttpRequestMessage RequestMessage { get; }
        public bool IsSuccessStatusCode { get; }
        public RateLimitInfo? RateLimit { get; }

        private readonly byte[] _body;
        public byte[] Bytes { get => _body; }
        public string Body { get => Encoding.UTF8.GetString(_body); }

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

        public T? JSON<T>()
        {
            using var sr = new StringReader(Body);
            using var reader = new JsonTextReader(sr);

            return Http.Serializer.Deserialize<T>(reader);
        }
    }
}
