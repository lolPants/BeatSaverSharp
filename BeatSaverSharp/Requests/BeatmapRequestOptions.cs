using System;
using System.Threading;

namespace BeatSaverSharp
{
    /// <summary>
    /// Options for a single beatmap request
    /// </summary>
    public sealed class BeatmapRequestOptions : IRequest, IRequestOptions
    {
        /// <summary>
        /// Cancellation token for this request
        /// </summary>
        public CancellationToken? Token { get; set; } = null;

        /// <summary>
        /// Progress reporter for this request
        /// </summary>
        public IProgress<double>? Progress { get; set; } = null;

        WebRequest IRequestOptions.CreateRequest(string url)
        {
            return new WebRequest(url)
            {
                Token = Token,
                Progress = Progress
            };
        }
    }
}
