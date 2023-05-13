namespace wan24.MappingObject
{
    /// <summary>
    /// Mapping object adapter
    /// </summary>
    /// <typeparam name="tSource">Source object type</typeparam>
    /// <typeparam name="tMain">Main object type</typeparam>
    public class MappingObjectAdapter<tSource, tMain> : MappingObjectBase<tSource>
        where tSource : class
        where tMain : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="main">Main object</param>
        public MappingObjectAdapter(tMain main) : base() => Main = main;

        /// <summary>
        /// Main object
        /// </summary>
        public tMain Main { get; }

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
