namespace BeatSaverSharp
{
    internal interface IPagedRequestOptions : IRequestOptions
    {
        int Page { get; }

        internal IPagedRequestOptions Clone(IRequest? options = null, int? page = null);
    }
}
