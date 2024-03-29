﻿namespace wan24.MappingObject
{
    /// <summary>
    /// Mapping configuration
    /// </summary>
    /// <remarks>
    /// Constructor
    /// </remarks>
    /// <param name="source">Source object type</param>
    /// <param name="main">Main object type</param>
    /// <param name="mappings">Mappings</param>
    public class MappingConfig(
        Type source,
        Type main,
        params Mapping[] mappings
            ) : ICloneable
    {
        /// <summary>
        /// Get/set a mapping
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>Mapping</returns>
        public Mapping? this[string id]
        {
            get => Mappings.FirstOrDefault(m => m.MainPropertyName == id);
            set
            {
                if (value == null)
                {
                    Mappings = Mappings.Where(m => m.MainPropertyName != id).ToArray();
                }
                else if (this[id] != null)
                {
                    int index = 0;
                    for (; index < Mappings.Length && Mappings[index].MainPropertyName != id; index++) ;
                    Mappings[index] = value;
                }
                else
                {
                    Mappings = [.. Mappings, .. new Mapping[] { value }];
                }
            }
        }

        /// <summary>
        /// Source object type
        /// </summary>
        public Type SourceType { get; } = source;

        /// <summary>
        /// Main object type
        /// </summary>
        public Type MainType { get; } = main;

        /// <summary>
        /// Mappings
        /// </summary>
        public Mapping[] Mappings { get; protected set; } = mappings;

        /// <summary>
        /// Before mapping handler
        /// </summary>
        public Mapping_Delegate? BeforeMapping { get; set; }

        /// <summary>
        /// After mapping handler
        /// </summary>
        public Mapping_Delegate? AfterMapping { get; set; }

        /// <summary>
        /// Before reverse mapping handler
        /// </summary>
        public Mapping_Delegate? BeforeReverseMapping { get; set; }

        /// <summary>
        /// After reverse mapping handler
        /// </summary>
        public Mapping_Delegate? AfterReverseMapping { get; set; }

        /// <inheritdoc/>
        public virtual object Clone() => new MappingConfig(SourceType, MainType, Mappings)
        {
            BeforeMapping = BeforeMapping,
            AfterMapping = AfterMapping,
            BeforeReverseMapping = BeforeReverseMapping,
            AfterReverseMapping = AfterReverseMapping
        };

        /// <summary>
        /// Delegate for a mapping handler
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="config">Configuration</param>
        public delegate void Mapping_Delegate(object source, object main, MappingConfig config);
    }
}
