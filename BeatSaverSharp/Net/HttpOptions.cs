using System;
using System.Collections.Generic;

namespace BeatSaverSharp
{
    /// <summary>
    /// HTTP Options
    /// </summary>
    public sealed class HttpOptions
    {
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);
        private static readonly bool _defaultHandleRateLimits = false;
        private static readonly List<HttpAgent> _defaultAgents = new();

#if NETSTANDARD2_1
        private static readonly Version _defaultHttpVersion = System.Net.HttpVersion.Version20;
#else
        private static readonly Version _defaultHttpVersion = System.Net.HttpVersion.Version11;
#endif

        /// <summary>
        /// Application Name
        /// </summary>
        public string ApplicationName { get; }

        /// <summary>
        /// Application Version
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// HTTP Timeout
        ///
        /// Defaults to 30 seconds
        /// </summary>
        public TimeSpan Timeout { get; }

        /// <summary>
        /// HTTP Version to use
        ///
        /// Defaults to HTTP/1.1 on .NET Standard 2.0 and HTTP/2 on .NET Standard 2.1
        /// </summary>
        public Version HttpVersion { get; }

        /// <summary>
        /// Whether to automatically catch and handle rate limit errors
        /// </summary>
        public bool HandleRateLimits { get; }

        /// <summary>
        /// HTTP User Agents for this Application
        /// </summary>
        public HttpAgent[] Agents { get; }

        /// <summary>
        /// Construct a new HTTP Options Struct
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="version">Application Version</param>
        /// <param name="timeout">HTTP Timeout</param>
        /// <param name="httpVersion">HTTP Version</param>
        /// <param name="handleRateLimits">Handle Rate Limits Automatically</param>
        /// <param name="agents">Additional User Agents</param>
        public HttpOptions(string? name, Version? version, TimeSpan? timeout = null, Version? httpVersion = null, bool? handleRateLimits = null, List<HttpAgent>? agents = null)
        {
            ApplicationName = name ?? throw new ArgumentNullException(nameof(name));
            Version = version?.ToString() ?? throw new ArgumentNullException(nameof(version));
            Timeout = timeout ?? _defaultTimeout;
            HttpVersion = httpVersion ?? _defaultHttpVersion;
            HandleRateLimits = handleRateLimits ?? _defaultHandleRateLimits;

            agents ??= _defaultAgents;
            Agents = agents.ToArray();
        }

        /// <summary>
        /// Construct a new HTTP Options Struct
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="version">Application Version</param>
        /// <param name="timeout">HTTP Timeout</param>
        /// <param name="httpVersion">HTTP Version</param>
        /// <param name="handleRateLimits">Handle Rate Limits Automatically</param>
        /// <param name="agents">Additional User Agents</param>
        public HttpOptions(string? name, string? version, TimeSpan? timeout = null, Version? httpVersion = null, bool? handleRateLimits = null, List<HttpAgent>? agents = null)
        {
            ApplicationName = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Timeout = timeout ?? _defaultTimeout;
            HttpVersion = httpVersion ?? _defaultHttpVersion;
            HandleRateLimits = handleRateLimits ?? _defaultHandleRateLimits;

            agents ??= _defaultAgents;
            Agents = agents.ToArray();
        }
    }
}
