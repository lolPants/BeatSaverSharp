using System;
using System.Threading;

namespace BeatSaverSharp
{
    /// <summary>
    /// Options for a paged beatmap request
    /// </summary>
    public sealed class SearchRequestOptions : IPagedRequestOptions, IRequest
    {
        /// <summary>
        /// Page Index
        /// </summary>
        public int Page { get; set; } = 0;

        /// <summary>
        /// Search Query
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Cancellation token for this request
        /// </summary>
        public CancellationToken? Token { get; set; } = null;

        /// <summary>
        /// Progress reporter for this request
        /// </summary>
        public IProgress<double>? Progress { get; set; } = null;

        /// <summary>
        /// Construct a new Search Request Options Struct
        /// </summary>
        /// <param name="query">Search Query</param>
        public SearchRequestOptions(string? query)
        {
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        WebRequest IRequestOptions.CreateRequest(string url)
        {
            var wr = new WebRequest($"{url}/{Page}")
            {
                Token = Token,
                Progress = Progress
            };

            if (Query is null) throw new NullReferenceException($"{nameof(Query)} should not be null!");
            wr.Query.Append("q", Query);

            return wr;
        }

        IPagedRequestOptions IPagedRequestOptions.Clone(IRequest? options, int? page)
        {
            var clone = new SearchRequestOptions(Query)
            {
                Token = options?.Token,
                Progress = options?.Progress,
            };

            if (page is not null) clone.Page = (int)page;
            return clone;
        }
    }
}
