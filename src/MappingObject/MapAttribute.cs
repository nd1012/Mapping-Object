namespace wan24.MappingObject
{
    /// <summary>
    /// Attribute for overriding the property mapping
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MapAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source object property name (use <code>nameof</code> here!)</param>
        public MapAttribute(string source) : base() => Source = source;

        /// <summary>
        /// Source object property name
        /// </summary>
        public string? Source { get; }
    }
}
