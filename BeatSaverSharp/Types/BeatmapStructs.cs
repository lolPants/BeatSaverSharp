using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    /// <summary>
    /// </summary>
    public struct Metadata
    {
        /// <summary>
        /// </summary>
        [JsonProperty("songName")]
        public string SongName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("songSubName")]
        public string SongSubName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("songAuthorName")]
        public string SongAuthorName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("levelAuthorName")]
        public string LevelAuthorName { get; set; }

        /// <summary>
        /// Duration of the Audio File (in seconds)
        /// </summary>
        [JsonProperty("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// Beats per Minute
        /// </summary>
        [JsonProperty("bpm")]
        public float BPM { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("difficulties")]
        public Difficulties Difficulties { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("characteristics")]
        public List<BeatmapCharacteristic> Characteristics { get; set; }
    }

    /// <summary>
    /// Available Difficulties
    /// </summary>
    public struct Difficulties
    {
        /// <summary>
        /// </summary>
        [JsonProperty("easy")]
        public bool Easy { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("normal")]
        public bool Normal { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("hard")]
        public bool Hard { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("expert")]
        public bool Expert { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("expertPlus")]
        public bool ExpertPlus { get; set; }
    }

    /// <summary>
    /// </summary>
    public struct BeatmapCharacteristic
    {
        /// <summary>
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("difficulties")]
        public Dictionary<string, BeatmapCharacteristicDifficulty?> Difficulties { get; set; }
    }

    /// <summary>
    /// </summary>
    public struct BeatmapCharacteristicDifficulty
    {
        /// <summary>
        /// Length of the beatmap (in beats)
        /// </summary>
        [JsonProperty("duration")]
        public float? Duration { get; set; }

        /// <summary>
        /// Length of the beatmap (in seconds)
        /// </summary>
        [JsonProperty("length")]
        public int? Length { get; set; }

        /// <summary>
        /// Bomb Count
        /// </summary>
        [JsonProperty("bombs")]
        public int? Bombs { get; set; }

        /// <summary>
        /// Note Count
        /// </summary>
        [JsonProperty("notes")]
        public int? Notes { get; set; }

        /// <summary>
        /// Obstacle Count
        /// </summary>
        [JsonProperty("obstacles")]
        public int? Obstacles { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("njs")]
        public float? NoteJumpSpeed { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("njsOffset")]
        public float? NoteJumpSpeedOffset { get; set; }
    }

    /// <summary>
    /// </summary>
    public struct Stats
    {
        /// <summary>
        /// </summary>
        [JsonProperty("downloads")]
        public int Downloads { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("plays")]
        public int Plays { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("upVotes")]
        public int UpVotes { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("downVotes")]
        public int DownVotes { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("rating")]
        public float Rating { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty("heat")]
        public float Heat { get; set; }
    }
}
