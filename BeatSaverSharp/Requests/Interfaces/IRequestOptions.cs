namespace BeatSaverSharp
{
    /// <summary>
    /// </summary>
    public interface IRequestOptions
    {
        internal WebRequest CreateRequest(string url);
    }
}
