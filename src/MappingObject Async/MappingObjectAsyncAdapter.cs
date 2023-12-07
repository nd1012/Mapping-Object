namespace wan24.MappingObject
{
    /// <summary>
    /// Mapping object adapter
    /// </summary>
    /// <typeparam name="tSource">Source object type</typeparam>
    /// <typeparam name="tMain">Main object type</typeparam>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="main">Main object</param>
    public class MappingObjectAsyncAdapter<tSource, tMain>(tMain main) : MappingObjectAsyncBase<tSource>()
        where tSource : class
        where tMain : class
    {
        /// <summary>
        /// Main object
        /// </summary>
        public tMain Main { get; } = main;

        /// <inheritdoc/>
        public override async Task MapFromAsync(tSource source, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            MappingConfig config = Mappings.EnsureMappings(typeof(tSource), Main.GetType());
            config.BeforeMapping?.Invoke(source, Main, config);
            foreach (Mapping map in config.Mappings)
                if (map is AsyncMapping asyncMap)
                {
                    await asyncMap.MapFromAsync(source, Main, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    map.MapFrom(source, Main);
                }
            config.AfterMapping?.Invoke(source, Main, config);
        }

        /// <inheritdoc/>
        public override async Task MapToAsync(tSource source, CancellationToken cancellationToken = default)
        {
            await Task.Yield();
            MappingConfig config = Mappings.EnsureMappings(typeof(tSource), Main.GetType());
            config.BeforeReverseMapping?.Invoke(source, Main, config);
            foreach (Mapping map in config.Mappings)
                if (map is AsyncMapping asyncMap)
                {
                    await asyncMap.MapToAsync(Main, source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else
                {
                    map.MapTo(Main, source);
                }
            config.AfterReverseMapping?.Invoke(source, Main, config);
        }
    }
}
