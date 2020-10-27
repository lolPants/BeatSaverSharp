using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// </summary>
    public sealed record Stats
    {
        /// <summary>
        /// </summary>
        [JsonProperty("downloads")]
        public int Downloads { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("plays")]
        public int Plays { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("upVotes")]
        public int UpVotes { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("downVotes")]
        public int DownVotes { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rating")]
        public float Rating { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("heat")]
        public float Heat { get; private set; }
    }
}
