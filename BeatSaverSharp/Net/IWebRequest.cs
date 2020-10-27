using System;
using System.Threading;

namespace BeatSaverSharp
{
    internal interface IWebRequest
    {
        string Uri { get; }
        CancellationToken? Token { get; }
        IProgress<double>? Progress { get; }
    }
}
