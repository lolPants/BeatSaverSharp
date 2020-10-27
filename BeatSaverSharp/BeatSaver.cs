using System;
using System.Net.Http;

namespace BeatSaverSharp
{
    /// <summary>
    /// BeatSaver API Methods
    /// </summary>
    public sealed class BeatSaver
    {
        /// <summary>
        /// Base URL for BeatSaver Instance
        /// </summary>
        public const string BaseURL = "https://beatsaver.com";

        /// <summary>
        /// Construct a new BeatSaver API Client
        /// </summary>
        /// <param name="options">HTTP Options</param>
        public BeatSaver(HttpOptions? options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));
            HttpInstance = new Http(options);
        }

        #region Properties
        internal Http HttpInstance { get; }
        internal HttpClient HttpClient { get => HttpInstance.Client; }
        #endregion
    }
}
