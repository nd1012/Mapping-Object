using static wan24.MappingObject.AsyncMapping;

namespace wan24.MappingObject
{
    /// <summary>
    /// Fluent asynchronous mapping extensions
    /// </summary>
    public static class AsyncFluentExtensions
    {
        /// <summary>
        /// Add a property mapping
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        /// <param name="sourceGetterAsync">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetterAsync">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        /// <param name="mapping">Mapping handler</param>
        /// <returns>Mappings</returns>
        public static MappingConfig WithAsyncMapping(
            this MappingConfig config, 
            string mainPropertyName, 
            string? sourcePropertyName, 
            AsyncSourceGetter_Delegate sourceGetterAsync,
            AsyncMainGetter_Delegate mainGetterAsync,
            AsyncMapping_Delegate? mapping = null
            )
        {
            config[mainPropertyName] = new AsyncMapping(mainPropertyName, sourcePropertyName, sourceGetterAsync, mainGetterAsync);
            mapping?.Invoke(config, (AsyncMapping)config[mainPropertyName]!);
            return config;
        }

        /// <summary>
        /// Add a custom mapping
        /// </summary>
        /// <param name="id">Mapping ID</param>
        /// <param name="config">Mapping configuration</param>
        /// <param name="sourceMapperAsync">Maps a source object value to the main object</param>
        /// <param name="mainMapperAsync">Maps a main object value to the source object</param>
        /// <returns>Mappings</returns>
        public static MappingConfig WithAsyncMapping(
            this MappingConfig config, 
            string id, 
            AsyncSourceMapper_Delegate sourceMapperAsync, 
            AsyncMainMapper_Delegate mainMapperAsync
            )
        {
            config[id] = new AsyncMapping(id, sourceMapperAsync, mainMapperAsync);
            return config;
        }

        /// <summary>
        /// Add a custom mapping
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="sourceMapperAsync">Maps a source object value to the main object</param>
        /// <param name="mainMapperAsync">Maps a main object value to the source object</param>
        /// <returns>Mappings</returns>
        public static MappingConfig WithAsyncMapping(this MappingConfig config, AsyncSourceMapper_Delegate sourceMapperAsync, AsyncMainMapper_Delegate mainMapperAsync)
        {
            string id = Guid.NewGuid().ToString();
            config[id] = new AsyncMapping(id, sourceMapperAsync, mainMapperAsync);
            return config;
        }

        /// <summary>
        /// Configure a mapping
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="id">ID (main object property name)</param>
        /// <param name="mapping">Mapping handler</param>
        /// <returns></returns>
        public static MappingConfig ConfigureAsyncMapping(this MappingConfig config, string id, AsyncMapping_Delegate mapping)
        {
            mapping(config, config[id] as AsyncMapping ?? throw new InvalidOperationException($"ID \"{id}\" is a synchronous mapping or wasn't found"));
            return config;
        }

        /// <summary>
        /// Set the source object property value converter
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="converter">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <returns>Mapping</returns>
        public static AsyncMapping WithAsyncSourceConverter(this AsyncMapping mapping, AsyncValueConverter_Delegate converter)
        {
            if (mapping.AsyncSourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.AsyncSourceConverter = converter;
            return mapping;
        }

        /// <summary>
        /// Set the main object property value converter
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="converter">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <returns>Mapping</returns>
        public static AsyncMapping WithAsyncMainConverter(this AsyncMapping mapping, AsyncValueConverter_Delegate converter)
        {
            if (mapping.AsyncSourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.AsyncMainConverter = converter;
            return mapping;
        }

        /// <summary>
        /// Set the source instance factory
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="factory">Source property value instance factory (used for automatic value type conversion with mapping)</param>
        /// <returns>Mapping</returns>
        public static AsyncMapping WithAsyncSourceInstanceFactory(this AsyncMapping mapping, AsyncSourceValueInstanceFactory_Delegate factory)
        {
            if (mapping.AsyncSourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.AsyncSourceInstanceFactory = factory;
            return mapping;
        }

        /// <summary>
        /// Set the main instance factory
        /// </summary>
        /// <param name="mapping">Mapping</param>
        /// <param name="factory">Main property value instance factory (used for automatic value type conversion with mapping)</param>
        /// <returns>Mapping</returns>
        public static AsyncMapping WithAsyncMainInstanceFactory(this AsyncMapping mapping, AsyncMainValueInstanceFactory_Delegate factory)
        {
            if (mapping.AsyncSourceMapper != null) throw new InvalidOperationException($"Manual mapping in effect for {mapping.MainPropertyName}");
            mapping.AsyncMainInstanceFactory = factory;
            return mapping;
        }

        /// <summary>
        /// Delegate for a mapping handler
        /// </summary>
        /// <param name="config">Mapping configuration</param>
        /// <param name="mapping">Mapping</param>
        public delegate void AsyncMapping_Delegate(MappingConfig config, AsyncMapping mapping);
    }
}
