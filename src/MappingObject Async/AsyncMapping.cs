namespace wan24.MappingObject
{
    /// <summary>
    /// Asynchronous property mapping definition
    /// </summary>
    public class AsyncMapping : Mapping
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        /// <param name="sourceGetter">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetter">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        [Obsolete("Asynchronous mapping doesn't support this constructor", error: true)]
        public AsyncMapping(string mainPropertyName, string? sourcePropertyName = null, SourceGetter_Delegate? sourceGetter = null, MainGetter_Delegate? mainGetter = null)
            => throw new NotSupportedException("Asynchronous mapping doesn't support this constructor");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Property name</param>
        /// <param name="sourceConverter">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <param name="mainConverter">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <param name="sourceGetter">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetter">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        [Obsolete("Asynchronous mapping doesn't support this constructor", error: true)]
        public AsyncMapping(
            string mainPropertyName,
            ValueConverter_Delegate sourceConverter,
            ValueConverter_Delegate? mainConverter = null,
            SourceGetter_Delegate? sourceGetter = null,
            MainGetter_Delegate? mainGetter = null
            )
            => throw new NotSupportedException("Asynchronous mapping doesn't support this constructor");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        /// <param name="sourceConverter">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <param name="mainConverter">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <param name="sourceGetter">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetter">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        [Obsolete("Asynchronous mapping doesn't support this constructor", error: true)]
        public AsyncMapping(
            string mainPropertyName,
            string? sourcePropertyName,
            ValueConverter_Delegate sourceConverter,
            ValueConverter_Delegate? mainConverter = null,
            SourceGetter_Delegate? sourceGetter = null,
            MainGetter_Delegate? mainGetter = null
            )
            => throw new NotSupportedException("Asynchronous mapping doesn't support this constructor");

        /// <summary>
        /// Constructr
        /// </summary>
        /// <param name="id">Mapping ID</param>
        /// <param name="sourceMapper">Maps a source object value to the main object</param>
        /// <param name="mainMapper">Maps a main object value to the source object</param>
        [Obsolete("Asynchronous mapping doesn't support this constructor", error: true)]
        public AsyncMapping(string id, SourceMapper_Delegate sourceMapper, MainMapper_Delegate mainMapper)
            => throw new NotSupportedException("Asynchronous mapping doesn't support this constructor");

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        /// <param name="sourceGetterAsync">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetterAsync">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        public AsyncMapping(string mainPropertyName, string? sourcePropertyName, AsyncSourceGetter_Delegate sourceGetterAsync, AsyncMainGetter_Delegate mainGetterAsync)
            : base(mainPropertyName, sourcePropertyName)
        {
            AsyncSourceGetter = sourceGetterAsync;
            AsyncMainGetter = mainGetterAsync;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Property name</param>
        /// <param name="sourceConverterAsync">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <param name="mainConverterAsync">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <param name="sourceGetterAsync">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetterAsync">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        public AsyncMapping(
            string mainPropertyName,
            AsyncValueConverter_Delegate sourceConverterAsync,
            AsyncValueConverter_Delegate? mainConverterAsync = null,
            AsyncSourceGetter_Delegate? sourceGetterAsync = null,
            AsyncMainGetter_Delegate? mainGetterAsync = null
            )
            : base(mainPropertyName, sourcePropertyName: null)
        {
            AsyncSourceConverter = sourceConverterAsync;
            AsyncMainConverter = mainConverterAsync;
            AsyncSourceGetter = sourceGetterAsync;
            AsyncMainGetter = mainGetterAsync;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mainPropertyName">Main object property name</param>
        /// <param name="sourcePropertyName">Source object property name</param>
        /// <param name="sourceConverterAsync">Source object value converter (convert the value of the source object for setting to the main object property)</param>
        /// <param name="mainConverterAsync">Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)</param>
        /// <param name="sourceGetterAsync">Source object property getter (get the value of the source object for setting the main object property value)</param>
        /// <param name="mainGetterAsync">Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)</param>
        public AsyncMapping(
            string mainPropertyName,
            string sourcePropertyName,
            AsyncValueConverter_Delegate sourceConverterAsync,
            AsyncValueConverter_Delegate? mainConverterAsync = null,
            AsyncSourceGetter_Delegate? sourceGetterAsync = null,
            AsyncMainGetter_Delegate? mainGetterAsync = null
            )
            : base(mainPropertyName, sourcePropertyName)
        {
            AsyncSourceConverter = sourceConverterAsync;
            AsyncMainConverter = mainConverterAsync;
            AsyncSourceGetter = sourceGetterAsync;
            AsyncMainGetter = mainGetterAsync;
        }

        /// <summary>
        /// Constructr
        /// </summary>
        /// <param name="id">Mapping ID</param>
        /// <param name="sourceMapperAsync">Maps a source object value to the main object</param>
        /// <param name="mainMapperAsync">Maps a main object value to the source object</param>
        public AsyncMapping(string id, AsyncSourceMapper_Delegate sourceMapperAsync, AsyncMainMapper_Delegate mainMapperAsync) : base(id, sourcePropertyName: null)
        {
            AsyncSourceMapper = sourceMapperAsync;
            AsyncMainMapper = mainMapperAsync;
        }

        /// <summary>
        /// Source object value converter (convert the value of the source object for setting to the main object property)
        /// </summary>
        public AsyncValueConverter_Delegate? AsyncSourceConverter { get; }

        /// <summary>
        /// Main object value converter (convert the value of the main object for setting to the source object property in a reverse mapping)
        /// </summary>
        public AsyncValueConverter_Delegate? AsyncMainConverter { get; }

        /// <summary>
        /// Source object property getter (get the value of the source object for setting the main object property value)
        /// </summary>
        public AsyncSourceGetter_Delegate? AsyncSourceGetter { get; }

        /// <summary>
        /// Main object property getter (get the value of the main object for setting the source object property value in a reverse mapping)
        /// </summary>
        public AsyncMainGetter_Delegate? AsyncMainGetter { get; }

        /// <summary>
        /// Maps a source object value to the main object
        /// </summary>
        public AsyncSourceMapper_Delegate? AsyncSourceMapper { get; }

        /// <summary>
        /// Maps a main object value to the source object
        /// </summary>
        public AsyncMainMapper_Delegate? AsyncMainMapper { get; }

        /// <inheritdoc/>
        [Obsolete($"Use {nameof(MapFromAsync)} instead")]
        public override void MapFrom(object source, object main) => throw new NotSupportedException($"Use {nameof(MapFromAsync)} instead");

        /// <inheritdoc/>
        [Obsolete($"Use {nameof(MapToAsync)} instead")]
        public override void MapTo(object main, object source) => throw new NotSupportedException($"Use {nameof(MapToAsync)} instead");

        /// <summary>
        /// Map the source object value to the main object property (use the source object value getter and converter, if any)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        public virtual async Task MapFromAsync(object source, object main, CancellationToken cancellationToken = default)
        {
            if (AsyncSourceMapper != null)
            {
                await AsyncSourceMapper(source, main, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                return;
            }
            var (SourceProperty, MainProperty) = GetProperties(source, main);
            object? value = AsyncSourceGetter == null
                ? SourceProperty.GetValue(source)
                : await AsyncSourceGetter(source, main, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            if (AsyncSourceConverter != null) value = await AsyncSourceConverter(value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            MainProperty.SetValue(main, value);
        }

        /// <summary>
        /// Map the main object value to the source object property (use the main object value converter and getter, if any; reverse mapping)
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        public virtual async Task MapToAsync(object main, object source, CancellationToken cancellationToken = default)
        {
            if (AsyncMainMapper != null)
            {
                await AsyncMainMapper(main, source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                return;
            }
            var (SourceProperty, MainProperty) = GetProperties(source, main);
            if (!(SourceProperty.SetMethod?.IsPublic ?? false))
                throw new MappingException($"Source property {source.GetType()}.{SourcePropertyName} needs a public setter");
            object? value = AsyncMainGetter == null
                ? MainProperty.GetValue(main)
                : await AsyncMainGetter(main, source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            if (AsyncMainConverter != null) value = await AsyncMainConverter(value, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
            SourceProperty.SetValue(source, value);
        }

        /// <summary>
        /// Delegte for a value converter
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Converted value</returns>
        public delegate Task<object?> AsyncValueConverter_Delegate(object? value, CancellationToken cancellationToken);

        /// <summary>
        /// Delegate for a source value getter
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Value of the source object to set to the main object</returns>
        public delegate Task<object?> AsyncSourceGetter_Delegate(object source, object main, CancellationToken cancellationToken);

        /// <summary>
        /// Delegate for a main value getter
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Value of the main object to set to the source object</returns>
        public delegate Task<object?> AsyncMainGetter_Delegate(object main, object source, CancellationToken cancellationToken);

        /// <summary>
        /// Delegate for a source object mapper (needs to set the value of the source object to the main object)
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public delegate Task AsyncSourceMapper_Delegate(object source, object main, CancellationToken cancellationToken);

        /// <summary>
        /// Delegate for a main object mapper (needs to set the value of the main object to the source object)
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public delegate Task AsyncMainMapper_Delegate(object main, object source, CancellationToken cancellationToken);
    }
}
