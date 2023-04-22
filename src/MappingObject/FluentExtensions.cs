using static wan24.MappingObject.Mapping;

namespace wan24.MappingObject
{
    /// <summary>
    /// Fluent mapping extensions
    /// </summary>
    public static class FluentExtensions
    {
        /// <summary>
        /// Exclude properties
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="mainPropertyNames">Property names (or custom mapping name(s))</param>
        /// <returns>Mappings</returns>
        public static MappingConfig ExcludeProperties(this MappingConfig config, params string[] mainPropertyNames)
        {
            foreach (string name in mainPropertyNames) config[name] = null;
            return config;
        }

        /// <summary>
        /// Add a property mapping
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        /// <param name="mapping">Mapping handler</param>
        /// <returns>Mappings</returns>
        public static MappingConfig WithMapping(this MappingConfig config, string mainPropertyName, string? sourcePropertyName, Mapping_Delegate? mapping = null)
        {
            config[mainPropertyName] = new(mainPropertyName, sourcePropertyName);
            mapping?.Invoke(config, config[mainPropertyName]!);
            return config;
        }

        /// <summary>
        /// Add a custom mapping
        /// </summary>
        /// <param name="id">Mapping ID</param>
        /// <param name="config">Mapping configuration</param>
        /// <param name="sourceMapper">Maps a source object value to the main object</param>
        /// <param name="mainMapper">Maps a main object value to the source object</param>
        /// <returns>Mappings</returns>
        public static MappingConfig WithMapping(this MappingConfig config, string id, SourceMapper_Delegate sourceMapper, MainMapper_Delegate mainMapper)
        {
            config[id] = new(id, sourceMapper, mainMapper);
            return config;
        }

        /// <summary>
        /// Add a custom mapping
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="sourceMapper">Maps a source object value to the main object</param>
        /// <param name="mainMapper">Maps a main object value to the source object</param>
        /// <returns>Mappings</returns>
        public static MappingConfig WithMapping(this MappingConfig config, SourceMapper_Delegate sourceMapper, MainMapper_Delegate mainMapper)
        {
            string id = Guid.NewGuid().ToString();
            config[id] = new(id, sourceMapper, mainMapper);
            return config;
        }

        /// <summary>
        /// Configure a mapping
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="id">ID (main object property name)</param>
        /// <param name="mapping">Mapping handler</param>
        /// <returns></returns>
        public static MappingConfig ConfigureMapping(this MappingConfig config, string id, Mapping_Delegate mapping)
        {
            mapping(config, config[id] ?? throw new MappingException($"Mapping ID \"{id}\" not found"));
            return config;
        }

        /// <summary>
        /// Set the source property value converter
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="converter">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <returns>Mapping</returns>
        public static Mapping WithSourceConverter(this Mapping mapping, ValueConverter_Delegate converter)
        {
            if (mapping.SourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.SourceConverter = converter;
            return mapping;
        }

        /// <summary>
        /// Set the main property value converter
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="converter">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <returns>Mapping</returns>
        public static Mapping WithMainConverter(this Mapping mapping, ValueConverter_Delegate converter)
        {
            if (mapping.SourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.MainConverter = converter;
            return mapping;
        }

        /// <summary>
        /// Set the source object property value getter
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="getter">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <returns>Mapping</returns>
        public static Mapping WithSourceGetter(this Mapping mapping, SourceGetter_Delegate getter)
        {
            if (mapping.SourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.SourceGetter = getter;
            return mapping;
        }

        /// <summary>
        /// Set the main object property value getter
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="getter">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        /// <returns>Mapping</returns>
        public static Mapping WithMainGetter(this Mapping mapping, MainGetter_Delegate getter)
        {
            if (mapping.SourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.MainGetter = getter;
            return mapping;
        }

        /// <summary>
        /// Set the source instance factory
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="factory">Source property value instance factory (used for automatic value type conversion with mapping)</param>
        /// <returns>Mapping</returns>
        public static Mapping WithSourceInstanceFactory(this Mapping mapping, SourceValueInstanceFactory_Delegate factory)
        {
            if (mapping.SourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.SourceInstanceFactory = factory;
            return mapping;
        }

        /// <summary>
        /// Set the main instance factory
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="factory">Main property value instance factory (used for automatic value type conversion with mapping)</param>
        /// <returns>Mapping</returns>
        public static Mapping WithMainInstanceFactory(this Mapping mapping, MainValueInstanceFactory_Delegate factory)
        {
            if (mapping.SourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.MainInstanceFactory = factory;
            return mapping;
        }

        /// <summary>
        /// Delegate for a mapping handler
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="mapping">Mapping</param>
        public delegate void Mapping_Delegate(MappingConfig config, Mapping mapping);
    }
}
