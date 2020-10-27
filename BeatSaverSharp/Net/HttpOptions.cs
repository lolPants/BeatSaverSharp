using System;
using System.Collections.Generic;

namespace BeatSaverSharp
{
    /// <summary>
    /// HTTP Options
    /// </summary>
    public struct HttpOptions
    {
        private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);
        private static readonly bool _defaultHandleRateLimits = false;
        private static readonly List<HttpAgent> _defaultAgents = new();

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
        /// <param name="handleRateLimits">Handle Rate Limits Automatically</param>
        /// <param name="agents">Additional User Agents</param>
        public HttpOptions(string? name, Version? version, TimeSpan? timeout = null, bool? handleRateLimits = null, List<HttpAgent>? agents = null)
        {
            ApplicationName = name ?? throw new ArgumentNullException(nameof(name));
            Version = version?.ToString() ?? throw new ArgumentNullException(nameof(version));
            Timeout = timeout ?? _defaultTimeout;
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
        /// <param name="handleRateLimits">Handle Rate Limits Automatically</param>
        /// <param name="agents">Additional User Agents</param>
        public HttpOptions(string? name, string? version, TimeSpan? timeout = null, bool? handleRateLimits = null, List<HttpAgent>? agents = null)
        {
            ApplicationName = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
            Timeout = timeout ?? _defaultTimeout;
            HandleRateLimits = handleRateLimits ?? _defaultHandleRateLimits;

            agents ??= _defaultAgents;
            Agents = agents.ToArray();
        }
    }
}
