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
        public Mapping(string mainPropertyName, string? sourcePropertyName = null, Func<object, object, object?>? sourceGetter = null, Func<object, object, object?>? mainGetter = null)
        {
            MainPropertyName = mainPropertyName;
            SourcePropertyName = sourcePropertyName ?? mainPropertyName;
            SourceGetter = sourceGetter;
            MainGetter = mainGetter;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Property name</param>
        /// <param name="sourceConverter">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <param name="mainConverter">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <param name="sourceGetter">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetter">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        public Mapping(
            string mainPropertyName,
            Func<object?, object?> sourceConverter,
            Func<object?, object?>? mainConverter = null,
            Func<object, object, object?>? sourceGetter = null,
            Func<object, object, object?>? mainGetter = null
            )
        {
            MainPropertyName = SourcePropertyName = mainPropertyName;
            SourceConverter = sourceConverter;
            MainConverter = mainConverter;
            SourceGetter = sourceGetter;
            MainGetter = mainGetter;
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
        public Mapping(
            string mainPropertyName,
            string sourcePropertyName,
            Func<object?, object?> sourceConverter,
            Func<object?, object?>? mainConverter = null,
            Func<object, object, object?>? sourceGetter = null,
            Func<object, object, object?>? mainGetter = null
            )
        {
            MainPropertyName = mainPropertyName;
            SourcePropertyName = sourcePropertyName;
            SourceConverter = sourceConverter;
            MainConverter = mainConverter;
            SourceGetter = sourceGetter;
            MainGetter = mainGetter;
        }

        /// <summary>
        /// Main object property name
        /// </summary>
        public string MainPropertyName { get; }

        /// <summary>
        /// Source object property name
        /// </summary>
        public string SourcePropertyName { get; }

        /// <summary>
        /// Source object value converter (convert the value of the source object for setting to the main object property)
        /// </summary>
        public Func<object?, object?>? SourceConverter { get; }

        /// <summary>
        /// Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)
        /// </summary>
        public Func<object?, object?>? MainConverter { get; }

        /// <summary>
        /// Source object property getter (get the value of the source object for setting the main object property value)
        /// </summary>
        public Func<object, object, object?>? SourceGetter { get; }

        /// <summary>
        /// Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)
        /// </summary>
        public Func<object, object, object?>? MainGetter { get; }

        /// <summary>
        /// Map the source object value to the main object property (use the source object value getter and converter, if any)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        public virtual void MapFrom(object source, object main)
        {
            var (SourceProperty, MainProperty) = GetProperties(source, main);
            object? value = SourceGetter == null ? SourceProperty.GetValue(source) : SourceGetter(source, main);
            if (SourceConverter != null) value = SourceConverter(value);
            MainProperty.SetValue(main, value);
        }

        /// <summary>
        /// Map the main object value to the source object property (use the main object value converter and getter, if any; reverse mapping)
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        public virtual void MapTo(object main, object source)
        {
            var (SourceProperty, MainProperty) = GetProperties(source, main);
            if (!(SourceProperty.SetMethod?.IsPublic ?? false))
                throw new MappingException($"Source property {source.GetType()}.{SourcePropertyName} needs a public setter");
            object? value = MainGetter == null ? MainProperty.GetValue(main) : MainGetter(main, source);
            if (MainConverter != null) value = MainConverter(value);
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
    }
}
