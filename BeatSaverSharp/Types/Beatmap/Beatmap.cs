using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BeatSaverSharp.Exceptions;

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
            Uploader = null!;
            Metadata = null!;
            Stats = null!;
            DirectDownload = null!;
            DownloadURL = null!;
            CoverURL = null!;
            Hash = null!;
        }

        /// <summary>
        /// Instantiate a partial Beatmap
        /// </summary>
        /// <param name="client">BeatSaver Client</param>
        /// <param name="key">Hex Key</param>
        /// <param name="hash">SHA1 Hash</param>
        /// <param name="name">Beatmap Name</param>
        public Beatmap(BeatSaver client, string? key = null, string? hash = null, string? name = null)
        {
            if (key is null && hash is null)
            {
                throw new ArgumentException($"{nameof(Key)} and {nameof(Hash)} cannot both be null");
            }

            Client = client;
            ID = null!;
            Key = null!;
            Name = null!;
            Uploader = null!;
            Metadata = null!;
            Stats = null!;
            DirectDownload = null!;
            DownloadURL = null!;
            CoverURL = null!;
            Hash = null!;

            if (key is not null) Key = key;
            if (hash is not null) Hash = hash;
            if (name is not null) Name = name;
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

        /// <summary>
        /// User who uploaded this beatmap
        /// </summary>
        [JsonProperty("uploader")]
        public User Uploader { get; private set; }

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
            if (Partial == false) return;
            if (Client is null) throw new NullReferenceException($"{nameof(Client)} should not be null!");
            if (Key is null && Hash is null) throw new ArgumentException($"{nameof(Key)} and {nameof(Hash)} cannot both be null");

            var map = Hash is not null
                ? await Client.Hash(Hash, options).ConfigureAwait(false)
                : await Client.Key(Key, options).ConfigureAwait(false);

            if (map is null)
            {
                if (Hash is not null) throw new InvalidPartialHashException(Hash);
                else if (Key is not null) throw new InvalidPartialKeyException(Key);
                else throw new InvalidPartialException();
            }

            ID = map.ID;
            Key = map.Key;
            Name = map.Name;
            Description = map.Description;
            Uploader = map.Uploader;
            Uploaded = map.Uploaded;
            Metadata = map.Metadata;
            Stats = map.Stats;
            DirectDownload = map.DirectDownload;
            DownloadURL = map.DownloadURL;
            CoverURL = map.CoverURL;
            Hash = map.Hash;
        }

        /// <summary>
        /// Fetch latest Beatmap values
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task Refresh(BeatmapRequestOptions? options = null)
        {
            if (Client is null) throw new NullReferenceException($"{nameof(Client)} should not be null!");
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
            if (Client is null) throw new NullReferenceException($"{nameof(Client)} should not be null!");
            var b = await Client.StatsFromHash(Hash, options ?? BeatmapRequestOptions.Default).ConfigureAwait(false);

            if (b is not null)
            {
                Stats = b.Stats;
            }
        }
        #endregion

        #region Equality
        /// <summary>
        /// Check for value equality
        /// </summary>
        /// <param name="b">Beatmap to compare against</param>
        /// <returns></returns>
        public bool Equals(Beatmap b)
        {
            if (b is null) return false;
            if (ReferenceEquals(this, b)) return true;
            if (GetType() != b.GetType()) return false;

            return (ID == b.ID) || (Key == b.Key) || (Hash == b.Hash);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (ID is not null) return ID.GetHashCode();
            if (Key is not null) return Key.GetHashCode();
            if (Hash is not null) return Hash.GetHashCode();

            throw new NullReferenceException($"{nameof(ID)}, {nameof(Key)}, and {nameof(Hash)} should not all be null!");
        }
        #endregion
    }
}
