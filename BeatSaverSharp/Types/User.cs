using System;
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
            throw new NullReferenceException("ID should not be null!");
        }
        #endregion
    }
}
