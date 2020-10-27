using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// </summary>
    public sealed record BeatmapCharacteristic
    {
        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; private set; } = null!;

        /// <summary>
        /// </summary>
        [JsonProperty("difficulties")]
        public ReadOnlyDictionary<string, BeatmapCharacteristicDifficulty?> Difficulties { get; private set; } = null!;
    }
}
