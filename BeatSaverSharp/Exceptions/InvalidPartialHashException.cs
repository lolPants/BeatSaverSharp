namespace BeatSaverSharp.Exceptions
{
    /// <summary>
    /// Thrown when trying to populate an invalid partial Beatmap with an invalid Hash
    /// </summary>
    public class InvalidPartialHashException : InvalidPartialException
    {
        /// <summary>
        /// Invalid Hash
        /// </summary>
        public string Hash { get; }

        /// <summary>
        /// </summary>
        /// <param name="hash"></param>
        public InvalidPartialHashException(string hash) : base()
        {
            Hash = hash;
        }

        /// <summary>
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="message"></param>
        public InvalidPartialHashException(string hash, string message) : base(message)
        {
            Hash = hash;
        }
    }
}
