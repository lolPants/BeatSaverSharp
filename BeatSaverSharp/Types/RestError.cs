using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// Represents a standard REST error
    /// </summary>
    public struct RestError
    {
        /// <summary>
        /// Error Code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; private set; }

        /// <summary>
        /// Error Identifier
        /// </summary>
        [JsonProperty("identifier")]
        public string Identifier { get; private set; }
    }
}
