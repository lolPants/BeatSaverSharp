using System;
using System.Threading;
using BeatSaverSharp.Interfaces;

namespace BeatSaverSharp
{
    /// <summary>
    /// Options for a single beatmap request
    /// </summary>
    public sealed class StandardRequestOptions : IRequest, IRequestOptions
    {
        /// <summary>
        /// Default Beatmap Request Options
        /// </summary>
        public static StandardRequestOptions Default { get => new StandardRequestOptions(); }

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
