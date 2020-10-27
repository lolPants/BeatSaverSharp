using System;

namespace BeatSaverSharp.Exceptions
{
    /// <summary>
    /// Thrown when trying to populate an invalid partial Beatmap
    /// </summary>
    public class InvalidPartialException : Exception
    {
        /// <summary>
        /// </summary>
        public InvalidPartialException() : base() { }

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        public InvalidPartialException(string message) : base(message) { }
    }
}
