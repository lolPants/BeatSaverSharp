using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Reflection;
using Newtonsoft.Json;

namespace BeatSaverSharp
{
    internal sealed class Http
    {
        internal static readonly JsonSerializer Serializer = new();

        internal HttpOptions Options { get; }
        internal HttpClient Client { get; }

        internal Http(HttpOptions? options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));

            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };

            Client = new HttpClient(handler)
            {
                BaseAddress = new Uri($"{BeatSaver.BaseURL}/api/"),
                Timeout = Options.Timeout,
            };

            string libVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string userAgent = $"{Options.ApplicationName}/{Options.Version} BeatSaverSharp/{libVersion}";

            foreach (var agent in Options.Agents)
            {
                userAgent += $" {agent.Name}/{agent.Version.ToString()}";
            }

            Client.DefaultRequestHeaders.Add("User-Agent", userAgent);
        }
    }
}
