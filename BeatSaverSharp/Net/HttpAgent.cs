using System;

namespace BeatSaverSharp
{
    /// <summary>
    /// HTTP User Agent
    /// </summary>
    public struct HttpAgent
    {
        /// <summary>
        /// User Agent Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// User Agent Version String
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Construct a new Http User Agent
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        public HttpAgent(string name, string version) => (Name, Version) = (name, version);

        /// <summary>
        /// Construct a new Http User Agent
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        public HttpAgent(string name, Version version) => (Name, Version) = (name, version.ToString());

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        public void Deconstruct(out string name, out string version)
        {
            name = Name;
            version = Version;
        }
    }
}
