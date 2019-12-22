using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeatSaverSharp.Types
{
    /// <summary>
    /// Interface for classes that contain information about the application making requests.
    /// </summary>
    public interface IHasRequestor
    {
        /// <summary>
        /// Contains info about the application making requests. Cannot be set to null.
        /// </summary>
        RequestorInfo RequestorInfo { get; set; }
    }
}
