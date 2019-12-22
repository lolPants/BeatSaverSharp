using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BeatSaverSharp
{
    /// <summary>
    /// Beat Saver API Methods
    /// </summary>
    public class BeatSaver
        : Types.IHasRequestor
    {
        /// <summary>
        /// BeatSaver Base URL
        /// </summary>
        public const string BaseURL = "https://beatsaver.com";

        private RequestorInfo _requestorInfo;
        /// <summary>
        /// Contains info about the application making requests. Cannot be set to null.
        /// </summary>
        public RequestorInfo RequestorInfo
        {
            get { return _requestorInfo; }
            set
            {
                if (value == null) return;
                _requestorInfo = value;
            }
        }

        /// <summary>
        /// Creates a new object for making BeatSaver requests. Only one needs to be created per application.
        /// </summary>
        /// <param name="requestorName">Name of the product making requests.</param>
        /// <param name="requestorVersion">Version of the product making requests.</param>
        public BeatSaver(string requestorName, string requestorVersion)
        {
            if (string.IsNullOrEmpty(requestorName)) throw new ArgumentNullException(nameof(requestorName), "requestorName cannot be null or empty.");
            if (string.IsNullOrEmpty(requestorVersion)) throw new ArgumentNullException(nameof(requestorVersion), "requestorVersion cannot be null or empty.");
            RequestorInfo = new RequestorInfo(requestorName, requestorVersion);
        }

        #region Fetch Methods
        #region Static Methods

        internal static async Task<Beatmap> FetchByKey(string key, RequestorInfo requestorInfo, CancellationToken token, IProgress<double> progress = null) => await FetchSingle($"maps/{SingleType.Key}/{key}", requestorInfo, token, progress);
        internal static async Task<Beatmap> FetchByKey(string key, RequestorInfo requestorInfo, IProgress<double> progress = null) => await FetchSingle($"maps/{SingleType.Key}/{key}", requestorInfo, CancellationToken.None, progress);
        internal static async Task<Beatmap> FetchByHash(string hash, RequestorInfo requestorInfo, CancellationToken token, IProgress<double> progress = null) => await FetchSingle($"maps/{SingleType.Hash}/{hash}", requestorInfo, token, progress);
        internal static async Task<Beatmap> FetchByHash(string hash, RequestorInfo requestorInfo, IProgress<double> progress = null) => await FetchSingle($"maps/{SingleType.Hash}/{hash}", requestorInfo, CancellationToken.None, progress);

        internal static async Task<Page> FetchPaged(string url, RequestorInfo requestorInfo, CancellationToken token, IProgress<double> progress = null)
        {
            var resp = await Http.GetAsync(url, requestorInfo, token, progress).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            return resp.JSON<Page>();
        }

        internal static async Task<Beatmap> FetchSingle(string url, RequestorInfo requestorInfo, CancellationToken token, IProgress<double> progress = null)
        {
            var resp = await Http.GetAsync(url, requestorInfo, token, progress).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            return resp.JSON<Beatmap>();
        }

        internal static async Task<Page> FetchMapsPage(string type, uint page, RequestorInfo requestorInfo, CancellationToken token, IProgress<double> progress = null)
        {
            Page p = await FetchPaged($"maps/{type}/{page}", requestorInfo, token, progress);
            p.PageURI = $"maps/{type}";

            return p;
        }


        internal static async Task<Page> FetchSearchPage(string searchType, string query, uint page, RequestorInfo requestorInfo, CancellationToken token, IProgress<double> progress = null)
        {
            if (query == null) throw new ArgumentNullException("query");

            string encoded = HttpUtility.UrlEncode(query);
            string pageURI = $"search/{searchType}";

            string url = $"{pageURI}/{page}?q={encoded}";
            Page p = await FetchPaged(url, requestorInfo, token, progress);

            p.Query = query;
            p.PageURI = pageURI;

            return p;
        }
        #endregion
        /// <summary>
        /// Fetch a page of Latest beatmaps
        /// </summary>
        /// <param name="page">Optional page index (defaults to 0)</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Latest(uint page = 0, IProgress<double> progress = null) => await FetchMapsPage(PageType.Latest, page, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Fetch a page of Latest beatmaps
        /// </summary>
        /// <param name="page">Page index</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Latest(uint page, CancellationToken token, IProgress<double> progress = null) => await FetchMapsPage(PageType.Latest, page, RequestorInfo, token, progress);

        /// <summary>
        /// Fetch a page of Hot beatmaps
        /// </summary>
        /// <param name="page">Optional page index (defaults to 0)</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Hot(uint page = 0, IProgress<double> progress = null) => await FetchMapsPage(PageType.Hot, page, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Fetch a page of Hot beatmaps
        /// </summary>
        /// <param name="page">Page index</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Hot(uint page, CancellationToken token, IProgress<double> progress = null) => await FetchMapsPage(PageType.Hot, page, RequestorInfo, token, progress);

        /// <summary>
        /// Fetch a page of beatmaps ordered by their Rating
        /// </summary>
        /// <param name="page">Optional page index (defaults to 0)</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Rating(uint page = 0, IProgress<double> progress = null) => await FetchMapsPage(PageType.Rating, page, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Fetch a page of beatmaps ordered by their Rating
        /// </summary>
        /// <param name="page">Page index</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Rating(uint page, CancellationToken token, IProgress<double> progress = null) => await FetchMapsPage(PageType.Rating, page, RequestorInfo, token, progress);

        /// <summary>
        /// Fetch a page of beatmaps ordered by their download count
        /// </summary>
        /// <param name="page">Optional page index (defaults to 0)</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Downloads(uint page = 0, IProgress<double> progress = null) => await FetchMapsPage(PageType.Downloads, page, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Fetch a page of beatmaps ordered by their download count
        /// </summary>
        /// <param name="page">Page index</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Downloads(uint page, CancellationToken token, IProgress<double> progress = null) => await FetchMapsPage(PageType.Downloads, page, RequestorInfo, token, progress);

        /// <summary>
        /// Fetch a page of beatmaps ordered by their play count
        /// </summary>
        /// <param name="page">Optional page index (defaults to 0)</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Plays(uint page = 0, IProgress<double> progress = null) => await FetchMapsPage(PageType.Plays, page, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Fetch a page of beatmaps ordered by their play count
        /// </summary>
        /// <param name="page">Page index</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Plays(uint page, CancellationToken token, IProgress<double> progress = null) => await FetchMapsPage(PageType.Plays, page, RequestorInfo, token, progress);

        /// <summary>
        /// Fetch a Beatmap by Key
        /// </summary>
        /// <param name="key">Hex Key</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Beatmap> Key(string key, IProgress<double> progress = null) => await FetchByKey(key, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Fetch a Beatmap by Key
        /// </summary>
        /// <param name="key">Hex Key</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Beatmap> Key(string key, CancellationToken token, IProgress<double> progress = null) => await FetchByKey(key, RequestorInfo, token, progress);

        /// <summary>
        /// Fetch a Beatmap by Hash
        /// </summary>
        /// <param name="hash">SHA1 Hash</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Beatmap> Hash(string hash, IProgress<double> progress = null) => await FetchByHash(hash, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Fetch a Beatmap by Hash
        /// </summary>
        /// <param name="hash">SHA1 Hash</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Beatmap> Hash(string hash, CancellationToken token, IProgress<double> progress = null) => await FetchByHash(hash, RequestorInfo, token, progress);

        /// <summary>
        /// Text Search
        /// </summary>
        /// <param name="query">Text Query</param>
        /// <param name="page">Optional page index (defaults to 0)</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Search(string query, uint page = 0, IProgress<double> progress = null) => await FetchSearchPage(SearchType.Text, query, page, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Text Search
        /// </summary>
        /// <param name="query">Text Query</param>
        /// <param name="page">Page index</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> Search(string query, uint page, CancellationToken token, IProgress<double> progress = null) => await FetchSearchPage(SearchType.Text, query, page, RequestorInfo, token, progress);

        /// <summary>
        /// Advanced Lucene Search
        /// </summary>
        /// <param name="query">Lucene Query</param>
        /// <param name="page">Optional page index (defaults to 0)</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> SearchAdvanced(string query, uint page = 0, IProgress<double> progress = null) => await FetchSearchPage(SearchType.Advanced, query, page, RequestorInfo, CancellationToken.None, progress);
        /// <summary>
        /// Advanced Lucene Search
        /// </summary>
        /// <param name="query">Lucene Query</param>
        /// <param name="page">Page index</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<Page> SearchAdvanced(string query, uint page, CancellationToken token, IProgress<double> progress = null) => await FetchSearchPage(SearchType.Advanced, query, page, RequestorInfo, token, progress);

        /// <summary>
        /// Fetch a User by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<User> User(string id, IProgress<double> progress = null) => await User(id, CancellationToken.None, progress);
        /// <summary>
        /// Fetch a User by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <param name="token">Cancellation token</param>
        /// <param name="progress">Optional progress reporter</param>
        /// <returns></returns>
        public async Task<User> User(string id, CancellationToken token, IProgress<double> progress = null)
        {
            var resp = await Http.GetAsync($"users/find/{id}", RequestorInfo, token, progress).ConfigureAwait(false);
            if (resp.StatusCode == HttpStatusCode.NotFound) return null;

            return resp.JSON<User>();
        }
        #endregion
    }
}
