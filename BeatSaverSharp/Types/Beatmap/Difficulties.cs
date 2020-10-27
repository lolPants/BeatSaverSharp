using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// Available Difficulties
    /// </summary>
    public sealed record Difficulties
    {
        /// <summary>
        /// </summary>
        [JsonProperty("easy")]
        public bool Easy { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("normal")]
        public bool Normal { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("hard")]
        public bool Hard { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("expert")]
        public bool Expert { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("expertPlus")]
        public bool ExpertPlus { get; private set; }
    }
}
