namespace wan24.MappingObject
{
    /// <summary>
    /// Base class for a mapping object
    /// </summary>
    /// <typeparam name="T">Source type (which this type can map from/to)</typeparam>
    public abstract class MappingObjectBase<T>
        where T : class
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected MappingObjectBase() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source object to map to this instance</param>
        protected MappingObjectBase(T source) => MapFrom(source);

        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        public virtual void MapFrom(T source)
        {
            MappingConfig config = Mappings.EnsureMappings(typeof(T), GetType());
            config.BeforeMapping?.Invoke(source, this, config);
            foreach (Mapping map in config.Mappings) map.MapFrom(source, this);
            config.AfterMapping?.Invoke(source, this, config);
        }

        /// <summary>
        /// Map this instance to a source object (reverse mapping)
        /// </summary>
        /// <param name="source">Source object</param>
        public virtual void MapTo(T source)
        {
            MappingConfig config = Mappings.EnsureMappings(typeof(T), GetType());
            config.BeforeReverseMapping?.Invoke(source, this, config);
            foreach (Mapping map in config.Mappings) map.MapTo(this, source);
            config.AfterReverseMapping?.Invoke(source, this, config);
        }
    }
}
