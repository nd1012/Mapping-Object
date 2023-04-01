using System.Reflection;

namespace wan24.MappingObject
{
    /// <summary>
    /// Property mapping definition
    /// </summary>
    public class Mapping
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        /// <param name="sourceGetter">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetter">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        /// <param name="sourceInstanceFactory">Source property value instance factory (used for automatic value type conversion with mapping)</param>
        /// <param name="mainInstanceFactory">Main property value instance factory (used for automatic value type conversion with mapping)</param>
        public Mapping(
            string mainPropertyName,
            string? sourcePropertyName = null,
            SourceGetter_Delegate? sourceGetter = null,
            MainGetter_Delegate? mainGetter = null,
            SourceValueInstanceFactory_Delegate? sourceInstanceFactory = null,
            MainValueInstanceFactory_Delegate? mainInstanceFactory = null
            )
            : this(mainPropertyName, sourcePropertyName)
        {
            SourceGetter = sourceGetter;
            MainGetter = mainGetter;
            SourceInstanceFactory = sourceInstanceFactory;
            MainInstanceFactory = mainInstanceFactory;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Property name</param>
        /// <param name="sourceConverter">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <param name="mainConverter">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <param name="sourceGetter">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetter">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        /// <param name="sourceInstanceFactory">Source property value instance factory (used for automatic value type conversion with mapping)</param>
        /// <param name="mainInstanceFactory">Main property value instance factory (used for automatic value type conversion with mapping)</param>
        public Mapping(
            string mainPropertyName,
            ValueConverter_Delegate sourceConverter,
            ValueConverter_Delegate? mainConverter = null,
            SourceGetter_Delegate? sourceGetter = null,
            MainGetter_Delegate? mainGetter = null,
            SourceValueInstanceFactory_Delegate? sourceInstanceFactory = null,
            MainValueInstanceFactory_Delegate? mainInstanceFactory = null
            )
            : this(mainPropertyName, sourcePropertyName: null)
        {
            SourceConverter = sourceConverter;
            MainConverter = mainConverter;
            SourceGetter = sourceGetter;
            MainGetter = mainGetter;
            SourceInstanceFactory = sourceInstanceFactory;
            MainInstanceFactory = mainInstanceFactory;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        /// <param name="sourceConverter">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <param name="mainConverter">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <param name="sourceGetter">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetter">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        /// <param name="sourceInstanceFactory">Source property value instance factory (used for automatic value type conversion with mapping)</param>
        /// <param name="mainInstanceFactory">Main property value instance factory (used for automatic value type conversion with mapping)</param>
        public Mapping(
            string mainPropertyName,
            string? sourcePropertyName,
            ValueConverter_Delegate sourceConverter,
            ValueConverter_Delegate? mainConverter = null,
            SourceGetter_Delegate? sourceGetter = null,
            MainGetter_Delegate? mainGetter = null,
            SourceValueInstanceFactory_Delegate? sourceInstanceFactory = null,
            MainValueInstanceFactory_Delegate? mainInstanceFactory = null
            )
            : this(mainPropertyName, sourcePropertyName)
        {
            SourceConverter = sourceConverter;
            MainConverter = mainConverter;
            SourceGetter = sourceGetter;
            MainGetter = mainGetter;
            SourceInstanceFactory = sourceInstanceFactory;
            MainInstanceFactory = mainInstanceFactory;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Mapping ID</param>
        /// <param name="sourceMapper">Maps a source object value to the main object</param>
        /// <param name="mainMapper">Maps a main object value to the source object</param>
        public Mapping(string id, SourceMapper_Delegate sourceMapper, MainMapper_Delegate mainMapper) : this(id, sourcePropertyName: null)
        {
            SourceMapper = sourceMapper;
            MainMapper = mainMapper;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected Mapping() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        protected Mapping(string mainPropertyName, string? sourcePropertyName)
        {
            MainPropertyName = mainPropertyName;
            SourcePropertyName = sourcePropertyName ?? mainPropertyName;
        }

        /// <summary>
        /// Main object property name
        /// </summary>
        public string MainPropertyName { get; } = null!;

        /// <summary>
        /// Source object property name
        /// </summary>
        public string SourcePropertyName { get; } = null!;

        /// <summary>
        /// Source object value converter (convert the value of the source object for setting to the main object property)
        /// </summary>
        public ValueConverter_Delegate? SourceConverter { get; }

        /// <summary>
        /// Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)
        /// </summary>
        public ValueConverter_Delegate? MainConverter { get; }

        /// <summary>
        /// Source object property getter (get the value of the source object for setting the main object property value)
        /// </summary>
        public SourceGetter_Delegate? SourceGetter { get; }

        /// <summary>
        /// Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)
        /// </summary>
        public MainGetter_Delegate? MainGetter { get; }

        /// <summary>
        /// Maps a source object value to the main object
        /// </summary>
        public SourceMapper_Delegate? SourceMapper { get; }

        /// <summary>
        /// Maps a main object value to the source object
        /// </summary>
        public MainMapper_Delegate? MainMapper { get; }

        /// <summary>
        /// Source property value instance factory (used for automatic value type conversion with mapping)
        /// </summary>
        public SourceValueInstanceFactory_Delegate? SourceInstanceFactory { get; }

        /// <summary>
        /// Main property value instance factory (used for automatic value type conversion with mapping)
        /// </summary>
        public MainValueInstanceFactory_Delegate? MainInstanceFactory { get; }

        /// <summary>
        /// Map the source object value to the main object property (use the source object value getter and converter, if any)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        public virtual void MapFrom(object source, object main)
        {
            if (SourceMapper != null)
            {
                SourceMapper(source, main);
                return;
            }
            var (SourceProperty, MainProperty) = GetProperties(source, main);
            object? value = SourceGetter == null ? SourceProperty.GetValue(source) : SourceGetter(source, main);
            if (SourceConverter != null) value = SourceConverter(value);
            Type mainType = MainProperty.PropertyType;
            if (
                value?.GetType() is Type valueType &&
                !mainType.IsAssignableFrom(valueType) &&
                !mainType.IsValueType &&
                (MainInstanceFactory != null || !mainType.IsInterface) &&
                !valueType.IsValueType
                )
                value = Mappings.MapFromObject(
                    value,
                    MainProperty.GetValue(main)
                        ?? MainInstanceFactory?.Invoke(value)
                        ?? Activator.CreateInstance(mainType)
                        ?? throw new MappingException($"Failed to instance main object property value type {mainType}")
                    );
            MainProperty.SetValue(main, value);
        }

        /// <summary>
        /// Map the main object value to the source object property (use the main object value converter and getter, if any; reverse mapping)
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        public virtual void MapTo(object main, object source)
        {
            if (MainMapper != null)
            {
                MainMapper(main, source);
                return;
            }
            var (SourceProperty, MainProperty) = GetProperties(source, main);
            if (!(SourceProperty.SetMethod?.IsPublic ?? false))
                throw new MappingException($"Source property {source.GetType()}.{SourcePropertyName} needs a public setter");
            object? value = MainGetter == null ? MainProperty.GetValue(main) : MainGetter(main, source);
            if (MainConverter != null) value = MainConverter(value);
            Type sourceType = SourceProperty.PropertyType;
            if (
                value?.GetType() is Type valueType &&
                !sourceType.IsAssignableFrom(valueType) &&
                !sourceType.IsValueType &&
                (SourceInstanceFactory != null || !sourceType.IsInterface) &&
                !valueType.IsValueType
                )
                value = Mappings.MapToObject(
                    value,
                    SourceProperty.GetValue(source)
                        ?? SourceInstanceFactory?.Invoke(value)
                        ?? Activator.CreateInstance(sourceType)
                        ?? throw new MappingException($"Failed to instance source object property value type {sourceType}")
                    );
            SourceProperty.SetValue(source, value);
        }

        /// <summary>
        /// Get source and main properties
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <returns>Source and main properties</returns>
        protected virtual (PropertyInfo SourceProperty, PropertyInfo MainProperty) GetProperties(object source, object main)
        {
            PropertyInfo spi = source.GetType().GetProperty(SourcePropertyName, BindingFlags.Instance | BindingFlags.Public)
                ?? throw new MappingException($"Source property {source.GetType()}.{SourcePropertyName} not found"),
                mpi = main.GetType().GetProperty(MainPropertyName, BindingFlags.Instance | BindingFlags.Public)
                ?? throw new MappingException($"Target property {main.GetType()}.{MainPropertyName} not found");
            if (!(spi.GetMethod?.IsPublic ?? false)) throw new MappingException($"Source property {source.GetType()}.{SourcePropertyName} needs a public getter");
            if (!(mpi.GetMethod?.IsPublic ?? false)) throw new MappingException($"Main property {main.GetType()}.{MainPropertyName} needs a public getter");
            if (!(mpi.SetMethod?.IsPublic ?? false)) throw new MappingException($"Main property {main.GetType()}.{MainPropertyName} needs a public setter");
            return (spi, mpi);
        }

        /// <summary>
        /// Delegate for a value converter
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>Converted value</returns>
        public delegate object? ValueConverter_Delegate(object? value);

        /// <summary>
        /// Delegate for a source value getter
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <returns>Value of the source object to set to the main object</returns>
        public delegate object? SourceGetter_Delegate(object source, object main);

        /// <summary>
        /// Delegate for a main value getter
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <returns>Value of the main object to set to the source object</returns>
        public delegate object? MainGetter_Delegate(object main, object source);

        /// <summary>
        /// Delegate for a source object mapper (needs to set the value of the source object to the main object)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        public delegate void SourceMapper_Delegate(object source, object main);

        /// <summary>
        /// Delegate for a main object mapper (needs to set the value of the main object to the source object)
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        public delegate void MainMapper_Delegate(object main, object source);

        /// <summary>
        /// Delegate for a main object target property value mapping instance factory method (used for automatic value type conversion with mapping)
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Instance</returns>
        public delegate object MainValueInstanceFactory_Delegate(object value);

        /// <summary>
        /// Delegate for a source object target property value mapping instance factory method (used for automatic value type conversion with mapping)
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns>Instance</returns>
        public delegate object SourceValueInstanceFactory_Delegate(object value);
    }
}
