using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// </summary>
    public struct BeatmapCharacteristicDifficulty
    {
        /// <summary>
        /// Length of the beatmap (in beats)
        /// </summary>
        [JsonProperty("duration")]
        public float Duration { get; private set; }

        /// <summary>
        /// Length of the beatmap (in seconds)
        /// </summary>
        [JsonProperty("length")]
        public long Length { get; private set; }

        /// <summary>
        /// Bomb Count
        /// </summary>
        [JsonProperty("bombs")]
        public int Bombs { get; private set; }

        /// <summary>
        /// Note Count
        /// </summary>
        [JsonProperty("notes")]
        public int Notes { get; private set; }

        /// <summary>
        /// Obstacle Count
        /// </summary>
        [JsonProperty("obstacles")]
        public int Obstacles { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("njs")]
        public float NoteJumpSpeed { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty("njsOffset")]
        public float NoteJumpSpeedOffset { get; private set; }
    }
}
