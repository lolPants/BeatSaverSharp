using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// Page of Beatmaps
    /// </summary>
    public sealed record Page<T> where T : class, IPagedRequestOptions, IRequest
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
        public uint LastPage { get; private set; }

        /// <summary>
        /// Index of the Previous Page
        /// </summary>
        [JsonProperty("prevPage")]
        public uint? PreviousPage { get; private set; }

        /// <summary>
        /// Index of the Next Page
        /// </summary>
        [JsonProperty("nextPage")]
        public uint? NextPage { get; private set; }
        #endregion

        #region Properties
        [JsonIgnore]
        internal BeatSaver? Client { get; set; }

        [JsonIgnore]
        internal string? URI { get; set; }

        [JsonIgnore]
        internal T? Options { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Fetch the next page in this sequence
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page<T>> Previous(T? options = null)
        {
            if (PreviousPage is null) throw new NullReferenceException($"{nameof(PreviousPage)} is null!");
            if (Client is null) throw new NullReferenceException($"{nameof(Client)} should not be null!");
            if (URI is null) throw new NullReferenceException($"{nameof(URI)} should not be null!");
            if (Options is null) throw new NullReferenceException($"{nameof(Options)} should not be null!");

            IPagedRequestOptions clone = Options.Clone(options, (uint)PreviousPage);
            var page = await Client.FetchPaged<T>(URI, clone).ConfigureAwait(false);

            return page!;
        }

        /// <summary>
        /// Fetch the next page in this sequence
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page<T>> Next(T? options = null)
        {
            if (NextPage is null) throw new NullReferenceException($"{nameof(NextPage)} is null!");
            if (Client is null) throw new NullReferenceException($"{nameof(Client)} should not be null!");
            if (URI is null) throw new NullReferenceException($"{nameof(URI)} should not be null!");
            if (Options is null) throw new NullReferenceException($"{nameof(Options)} should not be null!");

            IPagedRequestOptions clone = Options.Clone(options, (uint)NextPage);
            var page = await Client.FetchPaged<T>(URI, clone).ConfigureAwait(false);

            return page!;
        }
        #endregion
    }
}
