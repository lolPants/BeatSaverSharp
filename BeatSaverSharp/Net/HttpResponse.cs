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
        public T? JSON<T>()
        {
            string body = String();

            using StringReader sr = new StringReader(body);
            using JsonTextReader reader = new JsonTextReader(sr);

            return Http.Serializer.Deserialize<T>(reader);
        }
    }
}
