using System;

namespace BeatSaverSharp.Exceptions
{
    /// <summary>
    /// Thrown when an unknown exception occurs during voting
    /// </summary>
    public class UnknownVotingException : Exception
    {
        /// <summary>
        /// REST Error
        /// </summary>
        public RestError RestError { get; }

        /// <summary>
        /// </summary>
        /// <param name="restError">REST Error</param>
        public UnknownVotingException(RestError restError)
        {
            RestError = restError;
        }
    }
}
