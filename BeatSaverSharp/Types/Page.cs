using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// Page of Beatmaps
    /// </summary>
    public sealed record Page
    {
        #region JSON Properties
        /// <summary>
        /// Documents on this page
        /// </summary>
        [JsonProperty("docs")]
        public ReadOnlyCollection<Beatmap> Docs { get; private set; } = null!;

        /// <summary>
        /// Total number of documents for the specified endpoint
        /// </summary>
        [JsonProperty("totalDocs")]
        public int TotalDocs { get; private set; }

        /// <summary>
        /// Index of the Last Page
        /// </summary>
        [JsonProperty("lastPage")]
        public int LastPage { get; private set; }

        /// <summary>
        /// Index of the Previous Page
        /// </summary>
        [JsonProperty("prevPage")]
        public int? PreviousPage { get; private set; }

        /// <summary>
        /// Index of the Next Page
        /// </summary>
        [JsonProperty("nextPage")]
        public int? NextPage { get; private set; }
        #endregion

        #region Properties
        [JsonIgnore]
        internal BeatSaver? Client { get; set; }

        [JsonIgnore]
        internal PagedRequestOptions? Options { get; set; }
        #endregion
    }
}
