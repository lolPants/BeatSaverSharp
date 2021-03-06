using System;
using System.Threading;
using BeatSaverSharp.Interfaces;
using BeatSaverSharp.Net;

namespace BeatSaverSharp
{
    /// <summary>
    /// Options for a paged beatmap request
    /// </summary>
    public sealed class PagedRequestOptions : IPagedRequestOptions, IRequest
    {
        /// <summary>
        /// Default Paged Request Options
        /// </summary>
        public static PagedRequestOptions Default { get => new PagedRequestOptions(); }

        /// <summary>
        /// </summary>
        public enum AutomapFilter
        {
            /// <summary>
            /// Include automapped beatmaps in search results
            /// </summary>
            Include = 1,

            /// <summary>
            /// Exclude automapped beatmaps from search results
            /// </summary>
            Exclude = 0,

            /// <summary>
            /// Only display automapped beatmaps in search results
            /// </summary>
            Only = -1,
        }

        /// <summary>
        /// Page Index
        /// </summary>
        public uint Page { get; set; } = 0;

        /// <summary>
        /// Automap Filter
        /// <br />
        /// Defaults to Include
        /// </summary>
        public AutomapFilter Automaps { get; set; } = AutomapFilter.Include;

        /// <summary>
        /// Cancellation token for this request
        /// </summary>
        public CancellationToken? Token { get; set; } = null;

        /// <summary>
        /// Progress reporter for this request
        /// </summary>
        public IProgress<double>? Progress { get; set; } = null;

        HttpRequest IRequestOptions.CreateRequest(string url)
        {
            var wr = new HttpRequest($"{url}/{Page}")
            {
                Token = Token,
                Progress = Progress
            };

            if (Automaps == AutomapFilter.Include || Automaps == AutomapFilter.Only)
            {
                int value = (int)Automaps;
                wr.Query.Append("automapper", value.ToString());
            }

            return wr;
        }

        IPagedRequestOptions IPagedRequestOptions.Clone(IRequest? options, uint? page)
        {
            var clone = new PagedRequestOptions
            {
                Token = options?.Token,
                Progress = options?.Progress,
                Automaps = Automaps,
            };

            if (page is not null) clone.Page = (uint)page;
            return clone;
        }
    }
}
