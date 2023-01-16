namespace wan24.MappingObject
{
    /// <summary>
    /// Base class for an asynchronous mapping object
    /// </summary>
    /// <typeparam name="T">Source type (which this type can map from/to)</typeparam>
    public abstract class MappingObjectAsyncBase<T> : MappingObjectBase<T>
        where T : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected MappingObjectAsyncBase() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source object to map to this instance</param>
        protected MappingObjectAsyncBase(T source) => MapFrom(source);

        /// <inheritdoc/>
        public override void MapFrom(T source) => MapFromAsync(source).Wait();

        /// <inheritdoc/>
        public override void MapTo(T source) => MapToAsync(source).Wait();

        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public virtual async Task MapFromAsync(T source, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            MappingConfig config = Mappings.EnsureMappings(typeof(T), GetType());
            config.BeforeMapping?.Invoke(source, this, config);
            foreach (Mapping map in config.Mappings)
                if (map is AsyncMapping asyncMap)
                {
                    await asyncMap.MapFromAsync(source, this, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    map.MapFrom(source, this);
                }
            config.AfterMapping?.Invoke(source, this, config);
        }

        /// <summary>
        /// Map this instance to a source object (reverse mapping)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public virtual async Task MapToAsync(T source, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            MappingConfig config = Mappings.EnsureMappings(typeof(T), GetType());
            config.BeforeReverseMapping?.Invoke(source, this, config);
            foreach (Mapping map in config.Mappings)
                if (map is AsyncMapping asyncMap)
                {
                    await asyncMap.MapToAsync(this, source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    map.MapTo(this, source);
                }
            config.AfterReverseMapping?.Invoke(source, this, config);
        }
    }
}
