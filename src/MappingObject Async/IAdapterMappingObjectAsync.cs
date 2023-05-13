namespace wan24.MappingObject
{
    /// <summary>
    /// Interface for a mapping target object which uses a <see cref="MappingObjectAsyncAdapter{tSource, tMain}"/>
    /// </summary>
    /// <typeparam name="tSource">Source object type</typeparam>
    /// <typeparam name="tMain">Main object type</typeparam>
    public interface IAdapterMappingObjectAsync<tSource, tMain> : IAdapterMappingObject<tSource, tMain>
        where tSource : class
        where tMain : class
    {
        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task MapFromAsync(tSource source, CancellationToken cancellationToken = default);
        /// <summary>
        /// Map this instance to a source object (reverse mapping)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task MapToAsync(tSource source, CancellationToken cancellationToken = default);
    }
}
