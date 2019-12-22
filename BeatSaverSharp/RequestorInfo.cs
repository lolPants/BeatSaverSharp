using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaverSharp
{
    /// <summary>
    /// Contains info about the application making requests. Cannot be set to null.
    /// </summary>
    public class RequestorInfo
    {
        internal string UserAgent { get; set; }
        internal RequestorInfo(string requestorName, string requestorVersion)
        {
            UserAgent = string.Format(Http.BaseUserAgent, requestorName, requestorVersion);
        }
    }
}
