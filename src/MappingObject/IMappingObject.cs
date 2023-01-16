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
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal; <see langword="false"/>, if called from <code>Mappings.Map*</code>)</param>
        void MapFrom(object source, bool applyDefaultMappings = true);
        /// <summary>
        /// Map this instance to a source object (reverse mapping)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal; <see langword="false"/>, if called from <code>Mappings.Map*</code>)</param>
        void MapTo(object source, bool applyDefaultMappings = true);
    }

    /// <summary>
    /// Interface for a mapping object
    /// </summary>
    /// <typeparam name="T">Source object type</typeparam>
    public interface IMappingObject<T> : IMappingObject
        where T : class
    {
        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal; <see langword="false"/>, if called from <code>Mappings.Map*</code>)</param>
        void MapFrom(T source, bool applyDefaultMappings = true);
        /// <summary>
        /// Map this instance to a source object (reverse mapping)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="applyDefaultMappings">Apply default mappings, too? (used internal; <see langword="false"/>, if called from <code>Mappings.Map*</code>)</param>
        void MapTo(T source, bool applyDefaultMappings = true);
    }
}
