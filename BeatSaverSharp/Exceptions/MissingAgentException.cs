using System;

namespace BeatSaverSharp.Exceptions
{
    /// <summary>
    /// Thrown when an HTTP Options struct is created without any user agents
    /// </summary>
    public class MissingAgentException : Exception { }
}
