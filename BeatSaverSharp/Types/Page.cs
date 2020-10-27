using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
        internal string? URI { get; set; }

        [JsonIgnore]
        internal PagedRequestOptions? Options { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Fetch the next page in this sequence
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page> Previous(PagedRequestOptions? options = null)
        {
            if (PreviousPage is null) throw new NullReferenceException("PreviousPage is null!");
            if (Client is null) throw new NullReferenceException("Client should not be null!");
            if (URI is null) throw new NullReferenceException("URI should not be null!");
            if (Options is null) throw new NullReferenceException("Options should not be null!");

            var newOptions = options ?? PagedRequestOptions.Default;
            newOptions.Page = (int)PreviousPage;

            var page = await Client.FetchPaged(URI, newOptions).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch the next page in this sequence
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page> Next(PagedRequestOptions? options = null)
        {
            if (NextPage is null) throw new NullReferenceException("NextPage is null!");
            if (Client is null) throw new NullReferenceException("Client should not be null!");
            if (URI is null) throw new NullReferenceException("URI should not be null!");
            if (Options is null) throw new NullReferenceException("Options should not be null!");

            var newOptions = options ?? PagedRequestOptions.Default;
            newOptions.Page = (int)NextPage;

            var page = await Client.FetchPaged(URI, newOptions).ConfigureAwait(false);
            return page!;
        }
        #endregion
    }
}
