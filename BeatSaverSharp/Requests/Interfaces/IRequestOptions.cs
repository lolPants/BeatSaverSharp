namespace BeatSaverSharp.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IRequestOptions
    {
        internal WebRequest CreateRequest(string url);
    }
}
