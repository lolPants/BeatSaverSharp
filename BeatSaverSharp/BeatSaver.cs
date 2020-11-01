using System;
#if NETSTANDARD2_1
using System.Collections.Generic;
#endif
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BeatSaverSharp.Interfaces;
using BeatSaverSharp.Net;

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
        public static readonly string BaseURL = "https://beatsaver.com";

        #region Constructors
        /// <summary>
        /// Construct a new BeatSaver API Client
        /// </summary>
        /// <param name="options">HTTP Options</param>
        public BeatSaver(HttpOptions? options)
        {
            if (options is null) throw new ArgumentNullException(nameof(options));
            HttpInstance = new Http(options);
        }

        /// <summary>
        /// Construct a new BeatSaver API Client
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="version">Application Version</param>
        public BeatSaver(string? name, Version? version)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (version is null) throw new ArgumentNullException(nameof(version));

            var options = new HttpOptions(name, version);
            HttpInstance = new Http(options);
        }

        /// <summary>
        /// Construct a new BeatSaver API Client
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="version">Application Version</param>
        public BeatSaver(string? name, string? version)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));
            if (version is null) throw new ArgumentNullException(nameof(version));

            var options = new HttpOptions(name, version);
            HttpInstance = new Http(options);
        }
        #endregion

        #region Properties
        internal Http HttpInstance { get; }
        internal HttpClient HttpClient { get => HttpInstance.Client; }
        #endregion

        #region Internal Methods
        internal async Task<Beatmap?> FetchSingle(string url, StandardRequestOptions options)
        {
            var request = HttpRequest.FromOptions(url, options);
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

        internal async Task<Page<T>?> FetchPaged<T>(string url, IPagedRequestOptions options) where T : class, IPagedRequestOptions, IRequest
        {
            var request = HttpRequest.FromOptions(url, options);
            var resp = await HttpInstance.GetAsync(request).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            Page<T>? p = resp.JSON<Page<T>>();
            if (p is null) return null;

            p.Client = this;
            p.URI = url;
            p.Options = (T)options;

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
        public async Task<Page<PagedRequestOptions>> Latest(PagedRequestOptions? options = null)
        {
            var page = await FetchPaged<PagedRequestOptions>($"/maps/latest", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch a page of beatmaps, ordered by heat score
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page<PagedRequestOptions>> Hot(PagedRequestOptions? options = null)
        {
            var page = await FetchPaged<PagedRequestOptions>($"/maps/hot", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch a page of beatmaps, ordered by rating
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page<PagedRequestOptions>> Rating(PagedRequestOptions? options = null)
        {
            var page = await FetchPaged<PagedRequestOptions>($"/maps/rating", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch a page of beatmaps, ordered by download count
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page<PagedRequestOptions>> Downloads(PagedRequestOptions? options = null)
        {
            var page = await FetchPaged<PagedRequestOptions>($"/maps/downloads", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }
        #endregion

        #region Search Methods
        /// <summary>
        /// Fetch a page of beatmaps using standard text search
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page<SearchRequestOptions>> Search(SearchRequestOptions options)
        {
            var page = await FetchPaged<SearchRequestOptions>($"/search/text", options).ConfigureAwait(false);
            return page!;
        }

        /// <summary>
        /// Fetch a page of beatmaps using advanced Lucene syntax search
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page<SearchRequestOptions>> SearchAdvanced(SearchRequestOptions options)
        {
            var page = await FetchPaged<SearchRequestOptions>($"/search/advanced", options).ConfigureAwait(false);
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
            if (id is null) throw new ArgumentNullException(nameof(id));

            var request = HttpRequest.FromOptions($"/users/find/{id}", options ?? StandardRequestOptions.Default);
            var resp = await HttpInstance.GetAsync(request).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            User? u = resp.JSON<User>();
            if (u is null) return null;

            u.Client = this;
            return u;
        }
        #endregion

        #region Async Enumerables
#if NETSTANDARD2_1
        internal async IAsyncEnumerable<Beatmap> PageIterator(Task<Page<PagedRequestOptions>> firstTask, PagedRequestOptions? options)
        {
            Page<PagedRequestOptions>? maps = null;

            while (true)
            {
                if (maps is null) maps = await firstTask.ConfigureAwait(false);
                else maps = await maps.Next(null);

                foreach (var map in maps.Docs) yield return map;
                if (maps.NextPage is null) yield break;
            }
        }

        /// <summary>
        /// Return an async iterator, ordered by upload date
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public IAsyncEnumerable<Beatmap> LatestIterator(PagedRequestOptions? options = null)
        {
            return PageIterator(Latest(options), options);
        }

        /// <summary>
        /// Return an async iterator, ordered by heat score
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public IAsyncEnumerable<Beatmap> HotIterator(PagedRequestOptions? options = null)
        {
            return PageIterator(Hot(options), options);
        }

        /// <summary>
        /// Return an async iterator, ordered by rating
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public IAsyncEnumerable<Beatmap> RatingIterator(PagedRequestOptions? options = null)
        {
            return PageIterator(Rating(options), options);
        }

        /// <summary>
        /// Return an async iterator, ordered by download count
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public IAsyncEnumerable<Beatmap> DownloadsIterator(PagedRequestOptions? options = null)
        {
            return PageIterator(Downloads(options), options);
        }
#endif
        #endregion
    }
}
