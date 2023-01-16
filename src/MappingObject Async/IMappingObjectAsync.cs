namespace wan24.MappingObject
{
    /// <summary>
    /// Interface for an asynchronous mapping object
    /// </summary>
    public interface IMappingObjectAsync : IMappingObject
    {
        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal; <see langword="false"/>, if called from <code>Mappings.Map*</code>)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task MapFromAsync(object source, bool applyDefaultMappings = true, CancellationToken cancellationToken = default);
        /// <summary>
        /// Map this instance to a source object (reverse mapping)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal; <see langword="false"/>, if called from <code>Mappings.Map*</code>)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task MapToAsync(object source, bool applyDefaultMappings = true, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Interface for an asynchronous mapping object
    /// </summary>
    /// <typeparam name="T">Source object type</typeparam>
    public interface IMappingObjectAsync<T> : IMappingObjectAsync
        where T : class
    {
        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal; <see langword="false"/>, if called from <code>Mappings.Map*</code>)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task MapFromAsync(T source, bool applyDefaultMappings = true, CancellationToken cancellationToken = default);
        /// <summary>
        /// Map this instance to a source object (reverse mapping)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal; <see langword="false"/>, if called from <code>Mappings.Map*</code>)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task MapToAsync(T source, bool applyDefaultMappings = true, CancellationToken cancellationToken = default);
    }
}
