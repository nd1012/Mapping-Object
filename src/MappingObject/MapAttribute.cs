namespace wan24.MappingObject
{
    /// <summary>
    /// Attribute for overriding the automatic property mapping behavior
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MapAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source object property name to use (use <c>nameof</c> here!)</param>
        public MapAttribute(string source) : base() => Source = source;

        /// <summary>
        /// Source object property name to use
        /// </summary>
        public string Source { get; }
    }
}
