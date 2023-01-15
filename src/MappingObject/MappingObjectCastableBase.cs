namespace wan24.MappingObject
{
    /// <summary>
    /// Base class for a castable mapping object
    /// </summary>
    /// <typeparam name="tSource">Source type (which this type can map from/to)</typeparam>
    /// <typeparam name="tFinal">Final type which will be the target type for casting</typeparam>
    public abstract class MappingObjectCastableBase<tSource, tFinal> : MappingObjectBase<tSource>
        where tSource : class
        where tFinal : MappingObjectCastableBase<tSource, tFinal>, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected MappingObjectCastableBase() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source object to map to this instance</param>
        protected MappingObjectCastableBase(tSource source) : base(source) { }

        /// <summary>
        /// Cast a source object to this type
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>This type</returns>
        public static implicit operator MappingObjectCastableBase<tSource, tFinal>(tSource source)
        {
            tFinal res = new();
            res.MapFrom(source);
            return res;
        }
    }
}
