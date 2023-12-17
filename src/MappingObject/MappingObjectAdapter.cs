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
    public class MappingObjectAdapter<tSource, tMain>(tMain main) : MappingObjectBase<tSource>()
        where tSource : class
        where tMain : class
    {
        /// <summary>
        /// Main object
        /// </summary>
        public tMain Main { get; } = main;

        /// <inheritdoc/>
        public override void MapFrom(tSource source)
        {
            MappingConfig config = Mappings.EnsureMappings(typeof(tSource), Main.GetType());
            config.BeforeMapping?.Invoke(source, Main, config);
            foreach (Mapping map in config.Mappings) map.MapFrom(source, Main);
            config.AfterMapping?.Invoke(source, Main, config);
        }

        /// <inheritdoc/>
        public override void MapTo(tSource source)
        {
            MappingConfig config = Mappings.EnsureMappings(typeof(tSource), Main.GetType());
            config.BeforeReverseMapping?.Invoke(source, Main, config);
            foreach (Mapping map in config.Mappings) map.MapTo(Main, source);
            config.AfterReverseMapping?.Invoke(source, Main, config);
        }
    }
}
