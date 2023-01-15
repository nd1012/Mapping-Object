namespace wan24.MappingObject
{
    /// <summary>
    /// Attribute for not mapped properties
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SkipMappingAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SkipMappingAttribute() : base() { }
    }
}
