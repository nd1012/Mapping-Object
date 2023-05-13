namespace wan24.MappingObject
{
    /// <summary>
    /// Interface for a mapping target object which uses a <see cref="MappingObjectAdapter{tSource, tMain}"/>
    /// </summary>
    /// <typeparam name="tSource">Source object type</typeparam>
    /// <typeparam name="tMain">Main object type</typeparam>
    public interface IAdapterMappingObject<tSource, tMain>
        where tSource : class
        where tMain : class
    {
        /// <summary>
        /// Map a source object to this instance
        /// </summary>
        /// <param name="source">Source object</param>
        void MapFrom(tSource source);
        /// <summary>
        /// Map this instance to a source object (reverse mapping)
        /// </summary>
        /// <param name="source">Source object</param>
        void MapTo(tSource source);
    }
}
