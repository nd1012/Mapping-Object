namespace wan24.MappingObject
{
    /// <summary>
    /// Attribute for not automatic mapped properties (which require to be mapped customized, for example)
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
