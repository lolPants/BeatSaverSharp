using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// </summary>
    public sealed record Metadata
    {
        /// <summary>
        /// </summary>
        [JsonProperty("songName")]
        public string SongName { get; private set; } = null!;

        /// <summary>
        /// </summary>
        [JsonProperty("songSubName")]
        public string SongSubName { get; private set; } = null!;

        /// <summary>
        /// </summary>
        [JsonProperty("songAuthorName")]
        public string SongAuthorName { get; private set; } = null!;

        /// <summary>
        /// </summary>
        [JsonProperty("levelAuthorName")]
        public string LevelAuthorName { get; private set; } = null!;

        /// <summary>
        /// Duration of the Audio File (in seconds)
        /// </summary>
        [JsonProperty("duration")]
        public long Duration { get; private set; }

        /// <summary>
        /// Beats per Minute
        /// </summary>
        [JsonProperty("bpm")]
        public float BPM { get; private set; }

        /// <summary>
        /// Automapper Status
        /// </summary>
        [JsonProperty("automapper")]
        public string? Automapper { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("difficulties")]
        public Difficulties Difficulties { get; private set; } = null!;

        /// <summary>
        /// </summary>
        [JsonProperty("characteristics")]
        public ReadOnlyCollection<BeatmapCharacteristic> Characteristics { get; private set; } = null!;
    }
}
