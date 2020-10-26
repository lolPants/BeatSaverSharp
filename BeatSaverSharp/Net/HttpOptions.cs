using System;
using System.Collections.Generic;
using BeatSaverSharp.Exceptions;

namespace BeatSaverSharp
{
    /// <summary>
    /// HTTP Options
    /// </summary>
    public struct HttpOptions
    {
        /// <summary>
        /// Application Name
        /// </summary>
        public string ApplicationName { get; }

        /// <summary>
        /// Application Version
        /// </summary>
        public Version Version { get; }

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
        /// 
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="version">Application Version</param>
        /// <param name="agents">User Agents</param>
        /// <param name="timeout">HTTP Timeout</param>
        /// <param name="handleRateLimits">Handle Rate Limits Automatically</param>
        public HttpOptions(string name, Version version, List<HttpAgent> agents, TimeSpan? timeout = null, bool? handleRateLimits = null)
        {
            ApplicationName = name;
            Version = version;
            Timeout = timeout ?? TimeSpan.FromSeconds(30);
            HandleRateLimits = handleRateLimits ?? false;

            if (agents.Count == 0) throw new MissingAgentException();
            Agents = agents.ToArray();
        }
    }
}
