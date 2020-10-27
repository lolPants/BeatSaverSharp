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
    }
}
