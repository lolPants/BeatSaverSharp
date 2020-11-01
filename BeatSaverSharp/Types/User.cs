using System;
#if NETSTANDARD2_1
using System.Collections.Generic;
#endif
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// BeatSaver User
    /// </summary>
    public sealed record User
    {
        #region JSON Properties
        /// <summary>
        /// Unique ID
        /// </summary>
        [JsonProperty("_id")]
        public string ID { get; private set; } = null!;

        /// <summary>
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; private set; } = null!;
        #endregion

        #region Properties
        [JsonIgnore]
        internal BeatSaver? Client { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Fetch all Beatmaps uploaded by this user
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public async Task<Page<PagedRequestOptions>> Beatmaps(PagedRequestOptions? options = null)
        {
            if (Client is null) throw new NullReferenceException($"{nameof(Client)} should not be null!");

            var page = await Client.FetchPaged<PagedRequestOptions>($"/maps/uploader/{ID}", options ?? PagedRequestOptions.Default).ConfigureAwait(false);
            return page!;
        }
        #endregion

        #region Async Enumerables
#if NETSTANDARD2_1
        /// <summary>
        /// Return an async iterator for all beatmaps from this user
        /// </summary>
        /// <param name="options">Request Options</param>
        /// <returns></returns>
        public IAsyncEnumerable<Beatmap> BeatmapsIterator(PagedRequestOptions? options = null)
        {
            if (Client is null) throw new NullReferenceException($"{nameof(Client)} should not be null!");
            return Client.PageIterator(Beatmaps(options), options);
        }
#endif
        #endregion

        #region Equality
        /// <summary>
        /// Check for value equality
        /// </summary>
        /// <param name="u">User to compare against</param>
        /// <returns></returns>
        public bool Equals(User u)
        {
            if (u is null) return false;
            if (ReferenceEquals(this, u)) return true;
            if (GetType() != u.GetType()) return false;

            return (ID == u.ID);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (ID is not null) return ID.GetHashCode();
            throw new NullReferenceException($"{nameof(ID)} should not be null!");
        }
        #endregion
    }
}
