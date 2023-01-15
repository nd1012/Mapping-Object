namespace wan24.MappingObject
{
    /// <summary>
    /// Attribute for not automatic mapped properties (which may be mapped customized, for example)
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
