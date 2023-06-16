using System.Reflection;

namespace wan24.MappingObject
{
    /// <summary>
    /// Property mapping definition
    /// </summary>
    public class Mapping
    {
        /// <summary>
        /// CreateGetterDelegate method
        /// </summary>
        protected static readonly MethodInfo CreateGetterDelegateMethod;
        /// <summary>
        ///CreateSetterDelegate method
        /// </summary>
        protected static readonly MethodInfo CreateSetterDelegateMethod;

        /// <summary>
        /// Source property
        /// </summary>
        protected PropertyInfo? SourceProperty = null;
        /// <summary>
        /// Source property getter
        /// </summary>
        protected Func<object?, object?>? SourcePropertyGetter = null;
        /// <summary>
        /// Source property setter
        /// </summary>
        protected Action<object?, object?>? SourcePropertySetter = null;
        /// <summary>
        /// Main property
        /// </summary>
        protected PropertyInfo? MainProperty = null;
        /// <summary>
        /// Main property getter
        /// </summary>
        protected Func<object?, object?>? MainPropertyGetter = null;
        /// <summary>
        /// Main property setter
        /// </summary>
        protected Action<object?, object?>? MainPropertySetter = null;

        /// <summary>
        /// Static constructor
        /// </summary>
        static Mapping()
        {
            Type type = typeof(Mapping);
            CreateGetterDelegateMethod = type.GetMethod(nameof(CreateGetterDelegate), BindingFlags.NonPublic | BindingFlags.Static)
                ?? throw new InvalidProgramException($"Failed to reflect {typeof(Mapping)}.{nameof(CreateGetterDelegate)}");
            CreateSetterDelegateMethod = type.GetMethod(nameof(CreateSetterDelegate), BindingFlags.NonPublic | BindingFlags.Static)
                ?? throw new InvalidProgramException($"Failed to reflect {typeof(Mapping)}.{nameof(CreateSetterDelegate)}");
        }

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
        public ValueConverter_Delegate? SourceConverter { get; internal set; }

        /// <summary>
        /// Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)
        /// </summary>
        public ValueConverter_Delegate? MainConverter { get; internal set; }

        /// <summary>
        /// Source object property getter (get the value of the source object for setting the main object property value)
        /// </summary>
        public SourceGetter_Delegate? SourceGetter { get; internal set; }

        /// <summary>
        /// Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)
        /// </summary>
        public MainGetter_Delegate? MainGetter { get; internal set; }

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
        public SourceValueInstanceFactory_Delegate? SourceInstanceFactory { get; internal set; }

        /// <summary>
        /// Main property value instance factory (used for automatic value type conversion with mapping)
        /// </summary>
        public MainValueInstanceFactory_Delegate? MainInstanceFactory { get; internal set; }

        /// <summary>
        /// Map the source object value to the main object property (use the source object value getter and converter, if any)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        public virtual void MapFrom(object source, object main)
        {
            try
            {
                if (SourceMapper != null)
                {
                    SourceMapper(source, main);
                    return;
                }
                GetProperties(source, main);
                object? value = SourceGetter == null ? SourcePropertyGetter!(source) : SourceGetter(source, main);
                if (SourceConverter != null) value = SourceConverter(value);
                Type mainType = MainProperty!.PropertyType;
                if (
                    value?.GetType() is Type valueType &&
                    !mainType.IsAssignableFrom(valueType) &&
                    !mainType.IsValueType &&
                    (MainInstanceFactory != null || !mainType.IsInterface) &&
                    !valueType.IsValueType
                    )
                    value = Mappings.MapFromObject(
                        value,
                        MainPropertyGetter!(main)
                            ?? MainInstanceFactory?.Invoke(value)
                            ?? Activator.CreateInstance(mainType)
                            ?? throw new MappingException($"Failed to instance main object property value type {mainType}")
                        );
                MainPropertySetter!(main, value);
            }
            catch (MappingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MappingException($"Failed to map {source.GetType()}.{SourcePropertyName} to {main.GetType()}.{MainPropertyName}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Map the main object value to the source object property (use the main object value converter and getter, if any; reverse mapping)
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        public virtual void MapTo(object main, object source)
        {
            try
            {
                if (MainMapper != null)
                {
                    MainMapper(main, source);
                    return;
                }
                GetProperties(source, main);
                if (SourcePropertySetter == null)
                    throw new MappingException($"Source property {source.GetType()}.{SourcePropertyName} needs a public setter");
                object? value = MainGetter == null ? MainPropertyGetter!(main) : MainGetter(main, source);
                if (MainConverter != null) value = MainConverter(value);
                Type sourceType = SourceProperty!.PropertyType;
                if (
                    value?.GetType() is Type valueType &&
                    !sourceType.IsAssignableFrom(valueType) &&
                    !sourceType.IsValueType &&
                    (SourceInstanceFactory != null || !sourceType.IsInterface) &&
                    !valueType.IsValueType
                    )
                    value = Mappings.MapToObject(
                        value,
                        SourcePropertyGetter!(source)
                            ?? SourceInstanceFactory?.Invoke(value)
                            ?? Activator.CreateInstance(sourceType)
                            ?? throw new MappingException($"Failed to instance source object property value type {sourceType}")
                        );
                SourcePropertySetter(source, value);
            }
            catch (MappingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MappingException($"Failed to map {main.GetType()}.{MainPropertyName} to {source.GetType()}.{SourcePropertyName}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Get source and main properties and their getter/setter
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        protected virtual void GetProperties(object source, object main)
        {
            if (SourceProperty != null) return;
            SourceProperty = source.GetType().GetProperty(SourcePropertyName, BindingFlags.Instance | BindingFlags.Public)
                ?? throw new MappingException($"Source property {source.GetType()}.{SourcePropertyName} not found");
            MainProperty = main.GetType().GetProperty(MainPropertyName, BindingFlags.Instance | BindingFlags.Public)
                ?? throw new MappingException($"Target property {main.GetType()}.{MainPropertyName} not found");
            if (!(SourceProperty.GetMethod?.IsPublic ?? false)) throw new MappingException($"Source property {source.GetType()}.{SourcePropertyName} needs a public getter");
            if (!(MainProperty.GetMethod?.IsPublic ?? false)) throw new MappingException($"Main property {main.GetType()}.{MainPropertyName} needs a public getter");
            if (!(MainProperty.SetMethod?.IsPublic ?? false)) throw new MappingException($"Main property {main.GetType()}.{MainPropertyName} needs a public setter");
            SourcePropertyGetter = (Func<object?, object?>)CreateGetterDelegateMethod.MakeGenericMethod(
                    SourceProperty.DeclaringType!,
                    SourceProperty.PropertyType
                    ).Invoke(obj: null, new object[]{ Delegate.CreateDelegate(
                typeof(Func<,>).MakeGenericType(SourceProperty.DeclaringType!, SourceProperty.PropertyType),
                firstArgument: null,
                SourceProperty.GetMethod!
                ) })!;
            SourcePropertySetter = SourceProperty.CanWrite
                ? (Action<object?, object?>)CreateSetterDelegateMethod.MakeGenericMethod(
                    SourceProperty.DeclaringType!,
                    SourceProperty.PropertyType
                    ).Invoke(obj: null, new object[]{ Delegate.CreateDelegate(
                    typeof(Action<,>).MakeGenericType(SourceProperty.DeclaringType!, SourceProperty.PropertyType),
                    firstArgument: null,
                    SourceProperty.SetMethod!
                    ) })!
                : null;
            MainPropertyGetter = (Func<object?, object?>)CreateGetterDelegateMethod.MakeGenericMethod(
                    MainProperty.DeclaringType!,
                    MainProperty.PropertyType
                    ).Invoke(obj: null, new object[]{ Delegate.CreateDelegate(
                typeof(Func<,>).MakeGenericType(MainProperty.DeclaringType!, MainProperty.PropertyType),
                firstArgument: null,
                MainProperty.GetMethod!
                ) })!;
            MainPropertySetter = (Action<object?, object?>)CreateSetterDelegateMethod.MakeGenericMethod(
                MainProperty.DeclaringType!,
                MainProperty.PropertyType
                ).Invoke(obj: null, new object[]{ Delegate.CreateDelegate(
                typeof(Action<,>).MakeGenericType(MainProperty.DeclaringType!, MainProperty.PropertyType),
                firstArgument: null,
                MainProperty.SetMethod!
                ) })!;
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

        /// <summary>
        /// Create a getter delegate
        /// </summary>
        /// <typeparam name="tObject">Object type</typeparam>
        /// <typeparam name="tValue">Value type</typeparam>
        /// <param name="getter">Getter</param>
        /// <returns>Getter delegate</returns>
        protected static Func<object?, object?> CreateGetterDelegate<tObject, tValue>(Func<tObject?, tValue?> getter) => (obj) => getter((tObject)obj!);

        /// <summary>
        /// Create a setter delegate
        /// </summary>
        /// <typeparam name="tObject">Object type</typeparam>
        /// <typeparam name="tValue">Value type</typeparam>
        /// <param name="setter">Setter</param>
        /// <returns>Setter delegate</returns>
        protected static Action<object?, object?> CreateSetterDelegate<tObject, tValue>(Action<tObject?, tValue?> setter) => (obj, value) => setter((tObject)obj!, (tValue)value!);
    }
}
