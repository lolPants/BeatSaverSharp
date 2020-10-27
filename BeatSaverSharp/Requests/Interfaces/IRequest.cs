using System;
using System.Threading;

namespace BeatSaverSharp
{
    internal interface IRequest
    {
        CancellationToken? Token { get; }
        IProgress<double>? Progress { get; }
    }
}
