namespace wan24.MappingObject
{
    /// <summary>
    /// Thrown on any mapping error
    /// </summary>
    public class MappingException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MappingException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public MappingException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public MappingException(string message, Exception inner) : base(message, inner) { }

    }
}
