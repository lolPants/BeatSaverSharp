namespace BeatSaverSharp.Interfaces
{
    /// <summary>
    /// </summary>
    public interface IPagedRequestOptions : IRequestOptions
    {
        /// <summary>
        /// Page Index
        /// </summary>
        uint Page { get; }

        internal IPagedRequestOptions Clone(IRequest? options = null, uint? page = null);
    }
}
