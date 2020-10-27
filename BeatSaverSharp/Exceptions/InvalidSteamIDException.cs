using System;

namespace BeatSaverSharp.Exceptions
{
    /// <summary>
    /// Thrown when an invalid or mismatched SteamID is used to vote with
    /// </summary>
    public class InvalidSteamIDException : Exception
    {
        /// <summary>
        /// Invalid / Mismatched Steam ID
        /// </summary>
        public readonly string SteamID;

        /// <summary>
        /// </summary>
        /// <param name="steamID"></param>
        public InvalidSteamIDException(string steamID)
        {
            SteamID = steamID;
        }
    }
}
