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
        internal async Task<Beatmap?> FetchSingle(string url, StandardRequestOptions options)
        {
            var request = WebRequest.FromOptions(url, options);
            var resp = await HttpInstance.GetAsync(request).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            Beatmap? b = resp.JSON<Beatmap>();
            if (b is null) return null;

            b.Client = this;
            b.Uploader.Client = this;

            return b;
        }

        internal async Task<Beatmap?> StatsFromHash(string hash, StandardRequestOptions options)
        {
            return await FetchSingle($"/stats/hash/{hash}", options).ConfigureAwait(false);
        }

        internal async Task<Page?> FetchPaged(string url, IPagedRequestOptions options)
        {
            var request = WebRequest.FromOptions(url, options);
            var resp = await HttpInstance.GetAsync(request).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            Page? p = resp.JSON<Page>();
            if (p is null) return null;

            p.Client = this;
            p.URI = url;
            p.Options = options;

            foreach (var b in p.Docs)
            {
                b.Client = this;
                b.Uploader.Client = this;
            }

            return p;
        }
        #endregion

        #region Single Beatmap Methods
        /// <summary>
        /// Fetch a Beatmap by Key
        /// </summary>
        /// <param name="key">Hex Key</param>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Beatmap?> Key(string? key, StandardRequestOptions? options = null)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));
            return await FetchSingle($"/maps/detail/{key}", options ?? StandardRequestOptions.Default).ConfigureAwait(false);
        }

        /// <summary>
        /// Fetch a Beatmap by Hash
        /// </summary>
        /// <param name="hash">SHA1 Hash</param>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Beatmap?> Hash(string? hash, StandardRequestOptions? options = null)
        {
            if (hash is null) throw new ArgumentNullException(nameof(hash));
            return await FetchSingle($"/maps/by-hash/{hash}", options ?? StandardRequestOptions.Default).ConfigureAwait(false);
        }
        #endregion

        #region Paginated Methods
        /// <summary>
        /// Fetch a page of beatmaps, ordered by upload date
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page> Latest(PagedRequestOptions? options = null)
        {
            var page = await FetchPaged($"/maps/latest", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch a page of beatmaps, ordered by heat score
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page> Hot(PagedRequestOptions? options = null)
        {
            var page = await FetchPaged($"/maps/hot", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch a page of beatmaps, ordered by rating
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page> Rating(PagedRequestOptions? options = null)
        {
            var page = await FetchPaged($"/maps/rating", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch a page of beatmaps, ordered by download count
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page> Downloads(PagedRequestOptions? options = null)
        {
            var page = await FetchPaged($"/maps/downloads", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }
        #endregion

        #region Search Methods
        /// <summary>
        /// Fetch a page of beatmaps using standard text search
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page> Search(SearchRequestOptions options)
        {
            var page = await FetchPaged($"/search/text", options).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch a page of beatmaps using advanced Lucene syntax search
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page> SearchAdvanced(SearchRequestOptions options)
        {
            var page = await FetchPaged($"/search/advanced", options).ConfigureAwait(false);
            return page!;
        }
        #endregion

        #region User Method
        /// <summary>
        /// Fetch a User by ID
        /// </summary>
        /// <param name="id">Unique ID</param>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<User?> User(string? id, StandardRequestOptions? options = null)
        {
            var request = WebRequest.FromOptions($"/users/find/{id}", options ?? StandardRequestOptions.Default);
            var resp = await HttpInstance.GetAsync(request).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            User? u = resp.JSON<User>();
            if (u is null) return null;

            u.Client = this;
            return u;
        }
        #endregion
    }
}
