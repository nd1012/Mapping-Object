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
            foreach (Mapping map in Mappings.EnsureMappings(typeof(T), GetType())) map.MapFrom(source, this);
        }

        /// <summary>
        /// Map this instance to a source object
        /// </summary>
        /// <param name="source">Source object</param>
        public virtual void MapTo(T source)
        {
            foreach (Mapping map in Mappings.EnsureMappings(typeof(T), GetType())) map.MapTo(this, source);
        }
    }
}
