using System.Runtime.CompilerServices;

namespace wan24.MappingObject
{
    /// <summary>
    /// Asynchronous mapping extensions
    /// </summary>
    public static class AsyncMappingExtensions
    {
        /// <summary>
        /// Map a source object to a main object instance
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Main object</returns>
        public static async Task<tMain> MapFromAsync<tSource, tMain>(this tMain main, tSource source, MappingConfig? config = null, CancellationToken cancellationToken = default)
            where tSource : class
            where tMain : class
            => await AsyncMappings.MapFromAsync(source, main, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

        /// <summary>
        /// Map a main object to a source object instance (apply reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Source object</returns>
        public static async Task<tSource> MapToAsync<tMain, tSource>(this tSource source, tMain main, MappingConfig? config = null, CancellationToken cancellationToken = default)
            where tMain : class
            where tSource : class
            => await AsyncMappings.MapToAsync(main, source, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

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
                yield return await AsyncMappings.MapFromAsync(
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
                yield return await AsyncMappings.MapFromAsync(
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
                yield return await AsyncMappings.MapToAsync(
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
                yield return await AsyncMappings.MapToAsync(
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
                await AsyncMappings.MapFromAsync(pair.Key, pair.Value, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
                await AsyncMappings.MapFromAsync(pair.Key, pair.Value, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
                await AsyncMappings.MapToAsync(pair.Key, pair.Value, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
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
                await AsyncMappings.MapToAsync(pair.Key, pair.Value, config, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                yield return pair;
            }
        }
    }
}
