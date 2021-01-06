using System;
using System.Reflection;
using System.Text;

namespace BeatSaverSharp
{
    /// <summary>
    /// HTTP User Agent
    /// </summary>
    public struct HttpAgent
    {
        internal static readonly HttpAgent Self = new("BeatSaverSharp", Assembly.GetExecutingAssembly().GetName().Version.ToString(3), "https://github.com/lolPants/BeatSaverSharp");

        /// <summary>
        /// User Agent Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// User Agent Version String
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Optional User Agent Link
        ///
        /// Often used for Source Code repos
        /// </summary>
        public string? Link { get; }

        /// <summary>
        /// Construct a new HTTP User Agent Struct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="link"></param>
        public HttpAgent(string? name, string? version, string? link = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Link = link;
        }

        /// <summary>
        /// Construct a new HTTP User Agent Struct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="version"></param>
        /// <param name="link"></param>
        public HttpAgent(string? name, Version? version, string? link = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version?.ToString() ?? throw new ArgumentNullException(nameof(version));
            Link = link;
        }

        /// <summary>
        /// String representation of the constructed User Agent
        /// </summary>
        public string UserAgent
        {
            get
            {
                StringBuilder sb = new();
                sb.Append(Name)
                    .Append('/')
                    .Append(Version);

                if (Link is not null)
                {
                    sb.Append(" (+")
                        .Append(Link)
                        .Append(')');
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Alias for .UserAgent
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return UserAgent;
        }
    }
}
