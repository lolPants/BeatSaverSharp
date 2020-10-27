using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// BeatSaver Beatmap
    /// </summary>
    public sealed record Beatmap
    {
        #region Constructors
        /// <summary>
        /// Instantite a blank Beatmap
        /// </summary>
        [JsonConstructor]
        public Beatmap()
        {
            ID = null!;
            Key = null!;
            Name = null!;
            DirectDownload = null!;
            DownloadURL = null!;
            CoverURL = null!;
            Hash = null!;
        }
        #endregion

        #region JSON Properties
        /// <summary>
        /// Unique ID
        /// </summary>
        [JsonProperty("_id")]
        public string ID { get; private set; }

        /// <summary>
        /// Hex Key
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; }

        /// <summary>
        /// Multiline description
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; private set; }

        // TODO: Uploader Property

        /// <summary>
        /// Timestamp when this map was uploaded
        /// </summary>
        [JsonProperty("uploaded")]
        public DateTime Uploaded { get; private set; }

        /// <summary>
        /// Metadata for the Beatmap .dat file
        /// </summary>
        [JsonProperty("metadata")]
        public Metadata Metadata { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("stats")]
        public Stats Stats { get; private set; }

        /// <summary>
        /// Direct Download URL. Skips the download counter.
        /// </summary>
        [JsonProperty("directDownload")]
        public string DirectDownload { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("downloadURL")]
        public string DownloadURL { get; private set; }

        /// <summary>
        /// URL for the Cover Art
        /// </summary>
        [JsonProperty("coverURL")]
        public string CoverURL { get; private set; }

        /// <summary>
        /// SHA1 Hash
        /// </summary>
        [JsonProperty("hash")]
        public string Hash { get; private set; }
        #endregion

        #region Properties
        [JsonIgnore]
        internal BeatSaver? Client { get; set; }

        /// <summary>
        /// File name for the Cover Art
        /// </summary>
        [JsonIgnore]
        public string CoverFilename
        {
            get => Path.GetFileName(CoverURL);
        }

        /// <summary>
        /// Beatmap contains partial data.
        /// <br />
        /// Call <c>.Populate()</c> to fetch missing properties.
        /// </summary>
        [JsonIgnore]
        public bool Partial { get => ID is null || Name is null || CoverURL is null; }
        #endregion

        #region Methods
        /// <summary>
        /// Populate a partial beatmap with data
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task Populate(BeatmapRequestOptions? options = null)
        {
            if (Partial == false)
            {
                return;
            }

            // TODO: Implement
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetch latest Beatmap values
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task Refresh(BeatmapRequestOptions? options = null)
        {
            if (Client is null) throw new NullReferenceException("Client should not be null!");
            var b = await Client.StatsFromHash(Hash, options ?? BeatmapRequestOptions.Default).ConfigureAwait(false);

            if (b is not null)
            {
                Name = b.Name;
                Description = b.Description;
                Stats = b.Stats;
            }
        }

        /// <summary>
        /// Fetch latest Beatmap stats
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task RefreshStats(BeatmapRequestOptions? options = null)
        {
            if (Client is null) throw new NullReferenceException("Client should not be null!");
            var b = await Client.StatsFromHash(Hash, options ?? BeatmapRequestOptions.Default).ConfigureAwait(false);

            if (b is not null)
            {
                Stats = b.Stats;
            }
        }
        #endregion
    }
}
