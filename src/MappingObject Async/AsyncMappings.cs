using System.Reflection;
using System.Runtime.CompilerServices;

namespace wan24.MappingObject
{
    /// <summary>
    /// Automated asynchronous mappings
    /// </summary>
    public static class AsyncMappings
    {
        /// <summary>
        /// Task result property name
        /// </summary>
        private const string RESULT_PROPERTY_NAME = "Result";

        /// <summary>
        /// <c>MapFromAsync</c> method
        /// </summary>
        private static readonly MethodInfo MapFromAsyncMethod;
        /// <summary>
        /// <c>MapToAsync</c> method
        /// </summary>
        private static readonly MethodInfo MapToAsyncMethod;

        /// <summary>
        /// Constructor
        /// </summary>
        static AsyncMappings()
        {
            Type type = typeof(AsyncMappings);
            MapFromAsyncMethod = type.GetMethod("MapFromAsync", BindingFlags.Public | BindingFlags.Static) ?? throw new TypeLoadException("Failed to reflect the MapFromAsync method");
            MapToAsyncMethod = type.GetMethod("MapToAsync", BindingFlags.Public | BindingFlags.Static) ?? throw new TypeLoadException("Failed to reflect the MapToAsync method");
        }
        /// <summary>
        /// Map a source object to a main object instance
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Main object</returns>
        public static async Task<tMain> MapFromAsync<tSource, tMain>(tSource source, tMain main, MappingConfig? config = null, CancellationToken cancellationToken = default)
            where tSource : class
            where tMain : class
        {
            await Task.Yield();
            try
            {
                if(main is MappingObjectAsyncBase<tSource> mappingObjectAsync)
                {
                    await mappingObjectAsync.MapFromAsync(source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else if (main is MappingObjectBase<tSource> mappingObject)
                {
                    mappingObject.MapFrom(source);
                }
                else
                {
                    config ??= Mappings.EnsureMappings(source.GetType(), main.GetType());
                    config.BeforeMapping?.Invoke(source, main, config);
                    foreach (Mapping map in config.Mappings)
                        if (map is AsyncMapping asyncMap)
                        {
                            await asyncMap.MapFromAsync(source, main, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                        }
                        else
                        {
                            map.MapFrom(source, main);
                        }
                    if (main is IMappingObjectAsync<tSource> genericMappingObjectTypeAsync)
                    {
                        await genericMappingObjectTypeAsync.MapFromAsync(source, applyDefaultMappings: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                    }
                    else if (main is IMappingObjectAsync mappingObjectTypeAsync)
                    {
                        await mappingObjectTypeAsync.MapFromAsync(source, applyDefaultMappings: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                    }
                    else if (main is IMappingObject<tSource> genericMappingObjectType)
                    {
                        genericMappingObjectType.MapFrom(source, applyDefaultMappings: false);
                    }
                    else if (main is IMappingObject mappingObjectType)
                    {
                        mappingObjectType.MapFrom(source, applyDefaultMappings: false);
                    }
                    config.AfterMapping?.Invoke(source, main, config);
                }
            }
            catch (MappingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MappingException(message: null, ex);
            }
            return main;
        }

        /// <summary>
        /// Map a source object to a main object instance
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Main object</returns>
        public static async Task<object> MapFromObjectAsync(object source, object main, MappingConfig? config = null, CancellationToken cancellationToken = default)
        {
            await (Task)MapFromAsyncMethod.MakeGenericMethod(source.GetType(), main.GetType()).Invoke(obj: null, new object?[] { source, main, config, cancellationToken })!;
            return main;
        }

        /// <summary>
        /// Map a main object to a source object instance (apply reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Source object</returns>
        public static async Task<tSource> MapToAsync<tMain, tSource>(tMain main, tSource source, MappingConfig? config = null, CancellationToken cancellationToken = default)
            where tMain : class
            where tSource : class
        {
            await Task.Yield();
            try
            {
                if (main is MappingObjectAsyncBase<tSource> mappingObjectAsync)
                {
                    await mappingObjectAsync.MapToAsync(source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else if (main is MappingObjectBase<tSource> mappingObject)
                {
                    mappingObject.MapTo(source);
                }
                else
                {
                    config ??= Mappings.EnsureMappings(source.GetType(), main.GetType());
                    config.BeforeReverseMapping?.Invoke(source, main, config);
                    foreach (Mapping map in config.Mappings)
                        if (map is AsyncMapping asyncMap)
                        {
                            await asyncMap.MapToAsync(source, main, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                        }
                        else
                        {
                            map.MapTo(source, main);
                        }
                    if (main is IMappingObjectAsync<tSource> genericMappingObjectTypeAsync)
                    {
                        await genericMappingObjectTypeAsync.MapToAsync(source, applyDefaultMappings: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                    }
                    else if (main is IMappingObjectAsync mappingObjectTypeAsync)
                    {
                        await mappingObjectTypeAsync.MapToAsync(source, applyDefaultMappings: false, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                    }
                    if (main is IMappingObject<tSource> genericMappingObjectType)
                    {
                        genericMappingObjectType.MapTo(source, applyDefaultMappings: false);
                    }
                    else if (main is IMappingObject mappingObjectType)
                    {
                        mappingObjectType.MapTo(source, applyDefaultMappings: false);
                    }
                    config.AfterReverseMapping?.Invoke(source, main, config);
                }
            }
            catch (MappingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MappingException(message: null, ex);
            }
            return source;
        }

        /// <summary>
        /// Map a main object to a source object instance (apply reverse mapping)
        /// </summary>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Source object</returns>
        public static async Task<object> MapToObjectAsync(object main, object source, MappingConfig? config = null, CancellationToken cancellationToken = default)
        {
            await (Task)MapToAsyncMethod.MakeGenericMethod(main.GetType(), source.GetType()).Invoke(obj: null, new object?[] { main, source, config, cancellationToken })!;
            return source;
        }

        /// <summary>
        /// Map a list of source objects to a list of main objects
        /// </summary>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <param name="sources">Source objects</param>
        /// <param name="mainInstanceFactory">Main object instance factory</param>
        /// <param name="mainInstanceFactoryAsync">Asynchronous main object instance factory (will be preferred, if given)</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Main objects</returns>
        public static async IAsyncEnumerable<tMain> MapAllFromAsync<tSource, tMain>(
            this IEnumerable<tSource> sources,
            Func<tSource, tMain>? mainInstanceFactory = null,
            Func<tSource, CancellationToken, Task<tMain>>? mainInstanceFactoryAsync = null,
            MappingConfig? config = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            where tSource : class
            where tMain : class
        {
            if (mainInstanceFactoryAsync == null)
                mainInstanceFactory ??= (source) => (tMain)(Activator.CreateInstance(typeof(tMain)) ?? throw new MappingException($"Failed to instance {typeof(tMain)}"));
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            foreach (tSource source in sources)
                yield return await MapFromAsync(
                    source,
                    mainInstanceFactoryAsync == null
                        ? mainInstanceFactory!(source)
                        : await mainInstanceFactoryAsync(source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false),
                    config,
                    cancellationToken
                    ).ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// Map a list of source objects to a list of main objects
        /// </summary>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <param name="sources">Source objects</param>
        /// <param name="mainInstanceFactory">Main object instance factory</param>
        /// <param name="mainInstanceFactoryAsync">Asynchronous main object instance factory (will be preferred, if given)</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Main objects</returns>
        public static async IAsyncEnumerable<tMain> MapAllFromAsync<tSource, tMain>(
            this IAsyncEnumerable<tSource> sources,
            Func<tSource, tMain>? mainInstanceFactory = null,
            Func<tSource, CancellationToken, Task<tMain>>? mainInstanceFactoryAsync = null,
            MappingConfig? config = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            where tSource : class
            where tMain : class
        {
            if (mainInstanceFactoryAsync == null)
                mainInstanceFactory ??= (source) => (tMain)(Activator.CreateInstance(typeof(tMain)) ?? throw new MappingException($"Failed to instance {typeof(tMain)}"));
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            await foreach (tSource source in sources.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
                yield return await MapFromAsync(
                    source,
                    mainInstanceFactoryAsync == null
                        ? mainInstanceFactory!(source)
                        : await mainInstanceFactoryAsync(source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false),
                    config,
                    cancellationToken
                    ).ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// Map a list of main objects to a list of source objects (reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <param name="mainObjects">Main objects</param>
        /// <param name="sourceInstanceFactory">Source object instance factory</param>
        /// <param name="sourceInstanceFactoryAsync">Asynchronous source object instance factory (will be preferred, if given)</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Source objects</returns>
        public static async IAsyncEnumerable<tSource> MapAllToAsync<tMain, tSource>(
            this IEnumerable<tMain> mainObjects, 
            Func<tMain, tSource>? sourceInstanceFactory = null,
            Func<tMain, CancellationToken, Task<tSource>>? sourceInstanceFactoryAsync = null,
            MappingConfig? config = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            where tMain : class
            where tSource : class
        {
            if (sourceInstanceFactoryAsync == null)
                sourceInstanceFactory ??= (source) => (tSource)(Activator.CreateInstance(typeof(tSource)) ?? throw new MappingException($"Failed to instance {typeof(tSource)}"));
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            foreach (tMain main in mainObjects)
                yield return await MapToAsync(
                    main,
                    sourceInstanceFactoryAsync == null
                        ? sourceInstanceFactory!(main)
                        : await sourceInstanceFactoryAsync(main, cancellationToken).ConfigureAwait(continueOnCapturedContext: false),
                    config, 
                    cancellationToken
                    ).ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// Map a list of main objects to a list of source objects (reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <param name="mainObjects">Main objects</param>
        /// <param name="sourceInstanceFactory">Source object instance factory</param>
        /// <param name="sourceInstanceFactoryAsync">Asynchronous source object instance factory (will be preferred, if given)</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Source objects</returns>
        public static async IAsyncEnumerable<tSource> MapAllToAsync<tMain, tSource>(
            this IAsyncEnumerable<tMain> mainObjects,
            Func<tMain, tSource>? sourceInstanceFactory = null,
            Func<tMain, CancellationToken, Task<tSource>>? sourceInstanceFactoryAsync = null,
            MappingConfig? config = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            where tMain : class
            where tSource : class
        {
            if (sourceInstanceFactoryAsync == null)
                sourceInstanceFactory ??= (source) => (tSource)(Activator.CreateInstance(typeof(tSource)) ?? throw new MappingException($"Failed to instance {typeof(tSource)}"));
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            await foreach (tMain main in mainObjects.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
                yield return await MapToAsync(
                    main,
                    sourceInstanceFactoryAsync == null
                        ? sourceInstanceFactory!(main)
                        : await sourceInstanceFactoryAsync(main, cancellationToken).ConfigureAwait(continueOnCapturedContext: false),
                    config,
                    cancellationToken
                    ).ConfigureAwait(continueOnCapturedContext: false);
        }

        /// <summary>
        /// Map a list of source/main objects
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="pairs">Source/main objects</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Mapped source/main objects</returns>
        public static async IAsyncEnumerable<KeyValuePair<tSource, tMain>> MapAllFromAsync<tSource, tMain>(
            this IEnumerable<KeyValuePair<tSource, tMain>> pairs,
            MappingConfig? config = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            where tMain : class
            where tSource : class
        {
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            foreach (KeyValuePair<tSource, tMain> pair in pairs)
            {
                await MapFromAsync(pair.Key, pair.Value, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                yield return pair;
            }
        }

        /// <summary>
        /// Map a list of source/main objects
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="pairs">Source/main objects</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Mapped source/main objects</returns>
        public static async IAsyncEnumerable<KeyValuePair<tSource, tMain>> MapAllFromAsync<tSource, tMain>(
            this IAsyncEnumerable<KeyValuePair<tSource, tMain>> pairs,
            MappingConfig? config = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            where tMain : class
            where tSource : class
        {
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            await foreach (KeyValuePair<tSource, tMain> pair in pairs.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
            {
                await MapFromAsync(pair.Key, pair.Value, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                yield return pair;
            }
        }

        /// <summary>
        /// Map a list of main/source objects (reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="pairs">Main/source objects</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Mapped main/source objects</returns>
        public static async IAsyncEnumerable<KeyValuePair<tMain, tSource>> MapAllToAsync<tMain, tSource>(
            this IEnumerable<KeyValuePair<tMain, tSource>> pairs,
            MappingConfig? config = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            where tSource : class
            where tMain : class
        {
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            foreach (KeyValuePair<tMain, tSource> pair in pairs)
            {
                await MapToAsync(pair.Key, pair.Value, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                yield return pair;
            }
        }

        /// <summary>
        /// Map a list of main/source objects (reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="pairs">Main/source objects</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Mapped main/source objects</returns>
        public static async IAsyncEnumerable<KeyValuePair<tMain, tSource>> MapAllToAsync<tMain, tSource>(
            this IAsyncEnumerable<KeyValuePair<tMain, tSource>> pairs,
            MappingConfig? config = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default
            )
            where tSource : class
            where tMain : class
        {
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            await foreach (KeyValuePair<tMain, tSource> pair in pairs.WithCancellation(cancellationToken).ConfigureAwait(continueOnCapturedContext: false))
            {
                await MapToAsync(pair.Key, pair.Value, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                yield return pair;
            }
        }
    }
}
