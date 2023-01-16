namespace wan24.MappingObject
{
    /// <summary>
    /// Mapping configuration
    /// </summary>
    public class MappingConfig : ICloneable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source object type</param>
        /// <param name="main">Main object type</param>
        /// <param name="mappings">Mappings</param>
        public MappingConfig(
            Type source,
            Type main,
            params Mapping[] mappings
            )
        {
            SourceType = source;
            MainType = main;
            Mappings = mappings;
        }

        /// <summary>
        /// Source object type
        /// </summary>
        public Type SourceType { get; }

        /// <summary>
        /// Main object type
        /// </summary>
        public Type MainType { get; }

        /// <summary>
        /// Mappings
        /// </summary>
        public Mapping[] Mappings { get; }

        /// <summary>
        /// Before mapping handler
        /// </summary>
        public Mapping_Delegate? BeforeMapping { get; set; }

        /// <summary>
        /// After mapping handler
        /// </summary>
        public Mapping_Delegate? AfterMapping { get; set; }

        /// <summary>
        /// Before reverse mapping handler
        /// </summary>
        public Mapping_Delegate? BeforeReverseMapping { get; set; }

        /// <summary>
        /// After reverse mapping handler
        /// </summary>
        public Mapping_Delegate? AfterReverseMapping { get; set; }

        /// <inheritdoc/>
        public virtual object Clone() => new MappingConfig(SourceType, MainType, Mappings)
        {
            BeforeMapping = BeforeMapping,
            AfterMapping = AfterMapping,
            BeforeReverseMapping = BeforeReverseMapping,
            AfterReverseMapping = AfterReverseMapping
        };

        /// <summary>
        /// Delegate for a mapping handler
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="config">Configuration</param>
        public delegate void Mapping_Delegate(object source, object main, MappingConfig config);
    }
}
