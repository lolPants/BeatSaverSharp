using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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

        #region Internal Methods
        internal async Task<Beatmap?> FetchSingle(string url, BeatmapRequestOptions options)
        {
            var request = WebRequest.FromOptions(url, options);
            var resp = await HttpInstance.GetAsync(request).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            Beatmap? b = resp.JSON<Beatmap>();
            if (b is null) return null;

            b.Client = this;
            // TODO: Add client to beatmap uploader class
            // b.Uploader.Client = this;

            return b;
        }

        internal async Task<Beatmap?> StatsFromHash(string hash, BeatmapRequestOptions options)
        {
            return await FetchSingle($"/stats/hash/{hash}", options).ConfigureAwait(false);
        }
        #endregion

        #region Single Beatmap Methods
        /// <summary>
        /// Fetch a Beatmap by Key
        /// </summary>
        /// <param name="key">Hex Key</param>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Beatmap?> Key(string? key, BeatmapRequestOptions? options = null)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            return await FetchSingle($"/maps/detail/{key}", options ?? BeatmapRequestOptions.Default).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch a Beatmap by Hash
        /// </summary>
        /// <param name="hash">SHA1 Hash</param>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Beatmap?> Hash(string? hash, BeatmapRequestOptions? options = null)
        {
            if (hash is null) throw new ArgumentNullException(nameof(hash));
            return await FetchSingle($"/maps/by-hash/{hash}", options ?? BeatmapRequestOptions.Default).ConfigureAwait(false);
        }
        #endregion
    }
}
