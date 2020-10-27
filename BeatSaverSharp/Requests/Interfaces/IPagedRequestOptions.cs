namespace BeatSaverSharp
{
    internal interface IPagedRequestOptions : IRequestOptions
    {
        uint Page { get; }

        internal IPagedRequestOptions Clone(IRequest? options = null, uint? page = null);
    }
}
