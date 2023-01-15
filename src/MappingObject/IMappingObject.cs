namespace wan24.MappingObject
{
    /// <summary>
    /// Interface for a mapping object
    /// </summary>
    public interface IMappingObject
    {
        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal)</param>
        void MapFrom(object source, bool applyDefaultMappings = true);
        /// <summary>
        /// Map this instance to a source object
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal)</param>
        void MapTo(object source, bool applyDefaultMappings = true);
    }

    /// <summary>
    /// Interface for a mapping object
    /// </summary>
    public interface IMappingObject<T> : IMappingObject
        where T : class
    {
        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal)</param>
        void MapFrom(T source, bool applyDefaultMappings = true);
        /// <summary>
        /// Map this instance to a source object
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal)</param>
        void MapTo(T source, bool applyDefaultMappings = true);
    }
}
