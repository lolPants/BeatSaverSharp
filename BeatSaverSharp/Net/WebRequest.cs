using System;
using System.Threading;

namespace BeatSaverSharp
{
    internal struct WebRequest : IRequest
    {
        public static WebRequest FromOptions(string url, IRequestOptions options)
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

        public WebRequest(string? uri)
        {
            _uri = uri ?? throw new ArgumentException(nameof(uri));
            Query = new();
            Headers = new();

            Token = null;
            Progress = null;
        }
    }
}
