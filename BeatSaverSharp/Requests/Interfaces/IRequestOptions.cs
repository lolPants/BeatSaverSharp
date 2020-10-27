namespace BeatSaverSharp
{
    internal interface IRequestOptions
    {
        internal WebRequest CreateRequest(string url);
    }
}
