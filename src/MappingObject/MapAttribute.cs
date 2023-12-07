namespace wan24.MappingObject
{
    /// <summary>
    /// Attribute for overriding the automatic property mapping behavior
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="source">Source object property name to use (use <c>nameof</c> here!)</param>
    [AttributeUsage(AttributeTargets.Property)]
    public class MapAttribute(string source) : Attribute()
    {
        /// <summary>
        /// Source object property name to use
        /// </summary>
        public string Source { get; } = source;
    }
}
