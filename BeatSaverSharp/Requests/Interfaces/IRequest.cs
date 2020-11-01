using System;
using System.Threading;

namespace BeatSaverSharp
{
    /// <summary>
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Cancellation token for this request
        /// </summary>
        CancellationToken? Token { get; }

        /// <summary>
        /// Progress reporter for this request
        /// </summary>
        IProgress<double>? Progress { get; }
    }
}
