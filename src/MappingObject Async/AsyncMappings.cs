using System.Reflection;

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
                else if (main is IAdapterMappingObjectAsync<tSource, tMain> adapterMappingObjectAsync)
                {
                    await adapterMappingObjectAsync.MapFromAsync(source, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else if (main is IAdapterMappingObject<tSource, tMain> adapterMappingObject)
                {
                    adapterMappingObject.MapFrom(source);
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
                else if (main is IAdapterMappingObjectAsync<tSource, tMain> adapterMappingObjectAsync)
                {
                    await adapterMappingObjectAsync.MapToAsync(source, cancellationToken: cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
                }
                else if (main is IAdapterMappingObject<tSource, tMain> adapterMappingObject)
                {
                    adapterMappingObject.MapTo(source);
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
    }
}
