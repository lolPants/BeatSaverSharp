using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// BeatSaver User
    /// </summary>
    public sealed class User
        : Types.IHasRequestor
    {

        [JsonIgnore]
        private RequestorInfo _requestorInfo;
        /// <summary>
        /// Contains info about the application making requests. Cannot be set to null.
        /// </summary>
        [JsonIgnore]
        public RequestorInfo RequestorInfo {
            get { return _requestorInfo; }
            set {
                if (value == null) return;
                _requestorInfo = value;
            }
        }
        /// <summary>
        /// Unique ID
        /// </summary>
        [JsonProperty("_id")]
        public string ID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Fetch all Beatmaps uploaded by this user
        /// </summary>
        /// <param name="page">Optional page index (defaults to 0)</param>
        /// <param name="progress">Optional progress reporter</param>
        public async Task<Page> Beatmaps(uint page = 0, IProgress<double> progress = null) => await Beatmaps(page, CancellationToken.None, progress);
        /// <summary>
        /// Fetch all Beatmaps uploaded by this user
        /// </summary>
        /// <param name="page">Page index</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Beatmaps(uint page, CancellationToken token, IProgress<double> progress = null)
        {
            string pageURI = $"maps/{PageType.Uploader}/{ID}";
            string url = $"{pageURI}/{page}";

            Page p = await BeatSaver.FetchPaged(url, RequestorInfo, token, progress);
            p.PageURI = pageURI;

            return p;
        }
    }
}
