using System;
using System.Net.Http.Headers;
using System.Threading;

namespace BeatSaverSharp
{
    internal interface IRequest
    {
        CancellationToken? Token { get; }
        IProgress<double>? Progress { get; }
    }
}
