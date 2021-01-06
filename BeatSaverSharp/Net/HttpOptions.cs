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

#if NETSTANDARD2_1
        private static readonly Version _defaultHttpVersion = System.Net.HttpVersion.Version20;
#else
        private static readonly Version _defaultHttpVersion = System.Net.HttpVersion.Version11;
#endif

        /// <summary>
        /// HTTP Timeout
        /// <br />
        /// Defaults to 30 seconds
        /// </summary>
        public TimeSpan Timeout { get; }

        /// <summary>
        /// Base URL of BeatSaver Instance
        /// </summary>
        public string BaseURL { get; }

        /// <summary>
        /// HTTP Version to use
        /// <br />
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
        /// <param name="baseURL">Instance Base URL</param>
        /// <param name="httpVersion">HTTP Version</param>
        /// <param name="handleRateLimits">Handle Rate Limits Automatically</param>
        /// <param name="agents">Additional User Agents</param>
        public HttpOptions(
            string? name,
            Version? version,
            TimeSpan? timeout = null,
            string? baseURL = null,
            Version? httpVersion = null,
            bool? handleRateLimits = null,
            List<HttpAgent>? agents = null
        )
        {
            Timeout = timeout ?? _defaultTimeout;
            BaseURL = baseURL ?? BeatSaver.BaseURL;
            HttpVersion = httpVersion ?? _defaultHttpVersion;
            HandleRateLimits = handleRateLimits ?? _defaultHandleRateLimits;

            var appName = name ?? throw new ArgumentNullException(nameof(name));
            var appVersion = version ?? throw new ArgumentNullException(nameof(version));
            HttpAgent agent = new(appName, appVersion);

            Agents = ConstructAgents(agent, agents);
        }

        /// <summary>
        /// Construct a new HTTP Options Struct
        /// </summary>
        /// <param name="name">Application Name</param>
        /// <param name="version">Application Version</param>
        /// <param name="timeout">HTTP Timeout</param>
        /// <param name="baseURL">Instance Base URL</param>
        /// <param name="httpVersion">HTTP Version</param>
        /// <param name="handleRateLimits">Handle Rate Limits Automatically</param>
        /// <param name="agents">Additional User Agents</param>
        public HttpOptions(
            string? name,
            string? version,
            TimeSpan? timeout = null,
            string? baseURL = null,
            Version? httpVersion = null,
            bool? handleRateLimits = null,
            List<HttpAgent>? agents = null
        )
        {
            Timeout = timeout ?? _defaultTimeout;
            BaseURL = baseURL ?? BeatSaver.BaseURL;
            HttpVersion = httpVersion ?? _defaultHttpVersion;
            HandleRateLimits = handleRateLimits ?? _defaultHandleRateLimits;

            var appName = name ?? throw new ArgumentNullException(nameof(name));
            var appVersion = version ?? throw new ArgumentNullException(nameof(version));
            HttpAgent agent = new(appName, appVersion);

            Agents = ConstructAgents(agent, agents);
        }

        private static HttpAgent[] ConstructAgents(HttpAgent appAgent, List<HttpAgent>? agents)
        {
            List<HttpAgent> agentsList = new() { appAgent };
            if (agents is not null)
            {
                foreach (var agent in agents)
                {
                    if (agent.Name is null) throw new NullReferenceException($"{nameof(agent)}.{nameof(agent.Name)} must not be null!");
                    if (agent.Version is null) throw new NullReferenceException($"{nameof(agent)}.{nameof(agent.Version)} must not be null!");

                    agents.Add(agent);
                }
            }

            agentsList.Add(HttpAgent.Self);
            return agentsList.ToArray();
        }

        /// <summary>
        /// Combined User Agent String
        /// </summary>
        public string UserAgent
        {
            get => string.Join(" ", Agents);
        }
    }
}
