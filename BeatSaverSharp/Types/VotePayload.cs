using System;
using System.IO;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    internal struct VotePayload
    {
        internal enum VoteDirection : sbyte
        {
            Up = 1,
            Down = -1,
        }

        [JsonProperty("steamID")]
        public string SteamID { get; }

        [JsonIgnore]
        public byte[] Ticket { get; }
        [JsonProperty("ticket")]
        public string TicketString { get => string.Concat(Array.ConvertAll(Ticket, x => x.ToString("X2"))); }

        [JsonIgnore]
        public VoteDirection Direction { get; }
        [JsonProperty("direction")]
        public sbyte DirectionByte { get => (sbyte)Direction; }

        public VotePayload(string steamID, byte[] ticket, VoteDirection direction)
        {
            SteamID = steamID;
            Ticket = ticket;
            Direction = direction;
        }

        public string ToJson()
        {
            using var sw = new StringWriter();
            using var writer = new JsonTextWriter(sw);

            Http.Serializer.Serialize(writer, this);
            return sw.ToString();
        }
    }
}
