namespace BeatSaverSharp.Exceptions
{
    /// <summary>
    /// Thrown when trying to populate an invalid partial Beatmap with an invalid Key
    /// </summary>
    public class InvalidPartialKeyException : InvalidPartialException
    {
        /// <summary>
        /// Invalid Key
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        public InvalidPartialKeyException(string key) : base()
        {
            Key = key;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        public InvalidPartialKeyException(string key, string message) : base(message)
        {
            Key = key;
        }
    }
}
