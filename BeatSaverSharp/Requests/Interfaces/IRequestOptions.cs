using BeatSaverSharp.Net;

namespace BeatSaverSharp.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IRequestOptions
    {
        internal HttpRequest CreateRequest(string url);
    }
}
