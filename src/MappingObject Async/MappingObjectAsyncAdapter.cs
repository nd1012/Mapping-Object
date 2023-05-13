namespace wan24.MappingObject
{
    /// <summary>
    /// Mapping object adapter
    /// </summary>
    /// <typeparam name="tSource">Source object type</typeparam>
    /// <typeparam name="tMain">Main object type</typeparam>
    public class MappingObjectAsyncAdapter<tSource, tMain> : MappingObjectAsyncBase<tSource>
        where tSource : class
        where tMain : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="main">Main object</param>
        public MappingObjectAsyncAdapter(tMain main) : base() => Main = main;

        /// <summary>
        /// Main object
        /// </summary>
        public tMain Main { get; }

        /// <inheritdoc/>
        public override async Task MapFromAsync(tSource source, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            MappingConfig config = Mappings.EnsureMappings(typeof(tSource), Main.GetType());
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

        /// <inheritdoc/>
        public override async Task MapToAsync(tSource source, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            MappingConfig config = Mappings.EnsureMappings(typeof(tSource), Main.GetType());
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
