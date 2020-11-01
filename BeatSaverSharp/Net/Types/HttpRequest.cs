using System;
using System.Threading;
using BeatSaverSharp.Interfaces;

namespace BeatSaverSharp.Net
{
    internal struct HttpRequest : IRequest
    {
        public static HttpRequest FromOptions(string url, IRequestOptions options)
        {
            return options.CreateRequest(url);
        }

        private readonly string _uri;
        public string Uri
        {
            get
            {
                var query = Query.ToQueryString();
                if (query is null) return _uri;

                return $"{_uri}{query}";
            }
        }

        public QueryStore Query { get; }
        public MultiKeyDictionary<string, string> Headers { get; }

        public CancellationToken? Token { get; set; }
        public IProgress<double>? Progress { get; set; }

        public HttpRequest(string? uri)
        {
            _uri = uri?.TrimStart('/') ?? throw new ArgumentException(nameof(uri));
            Query = new();
            Headers = new();

            Token = null;
            Progress = null;
        }
    }
}
