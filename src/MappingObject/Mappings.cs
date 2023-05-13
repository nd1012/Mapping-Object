using System.Collections.Concurrent;
using System.Reflection;

namespace wan24.MappingObject
{
    /// <summary>
    /// Automated mappings
    /// </summary>
    public static class Mappings
    {
        /// <summary>
        /// Registered mappings
        /// </summary>
        private static readonly ConcurrentDictionary<string, MappingConfig> _Mappings = new();
        /// <summary>
        /// <c>MapFrom</c> method
        /// </summary>
        private static readonly MethodInfo MapFromMethod;
        /// <summary>
        /// <c>MapTo</c> method
        /// </summary>
        private static readonly MethodInfo MapToMethod;

        /// <summary>
        /// Constructor
        /// </summary>
        static Mappings()
        {
            Type type = typeof(Mappings);
            MapFromMethod = type.GetMethod("MapFrom", BindingFlags.Public | BindingFlags.Static) ?? throw new TypeLoadException("Failed to reflect the MapFrom method");
            MapToMethod = type.GetMethod("MapTo", BindingFlags.Public | BindingFlags.Static) ?? throw new TypeLoadException("Failed to reflect the MapTo method");
        }

        /// <summary>
        /// Types (keys) of registered mappings (the first type is the main type name, the second type is the source type name)
        /// </summary>
        public static IEnumerable<string> Types => _Mappings.Keys;

        /// <summary>
        /// Create automatic mappings
        /// </summary>
        /// <param name="source">Source object type</param>
        /// <param name="main">Main object type</param>
        /// <returns>Mappings</returns>
        public static Dictionary<string, Mapping> Create(Type source, Type main)
        {
            Dictionary<string, Mapping> res = new();
            MapAttribute? attr;
            foreach (PropertyInfo mpi in from mpi in main.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                         where (mpi.GetMethod?.IsPublic ?? false) &&
                                         (mpi.SetMethod?.IsPublic ?? false) &&
                                         mpi.GetCustomAttribute<SkipMappingAttribute>() == null
                                         select mpi)
            {
                attr = mpi.GetCustomAttribute<MapAttribute>();
                if (source.GetProperty(attr?.Source ?? mpi.Name, BindingFlags.Instance | BindingFlags.Public) is not PropertyInfo spi)
                {
                    if (attr?.Source != null) throw new MappingException($"Property {source}.{attr.Source} not found");
                    continue;
                }
                if (!(spi.GetMethod?.IsPublic ?? false))
                {
                    if (attr?.Source != null) throw new MappingException($"Property {source}.{spi.Name} needs a public getter");
                    continue;
                }
                res[mpi.Name] = new(mpi.Name, attr?.Source);
            }
            return res;
        }

        /// <summary>
        /// Add a mapping (creating automatic mappings)
        /// </summary>
        /// <param name="source">Source type</param>
        /// <param name="main">Main type</param>
        /// <param name="mappings">Overriding mappings (<see cref="Mapping.MainPropertyName"/> is used as the key)</param>
        /// <returns>Registered mapping configuration</returns>
        public static MappingConfig Add(Type source, Type main, params Mapping[] mappings)
        {
            Dictionary<string, Mapping> maps = Create(source, main);
            foreach (Mapping map in mappings) maps[map.MainPropertyName] = map;
            MappingConfig res = new(source, main, maps.Values.ToArray());
            _Mappings[$"{main} - {source}"] = res;
            return res;
        }

        /// <summary>
        /// Add a complete mapping configuration
        /// </summary>
        /// <param name="config">Mapping configuration (without any mappings to remove an existing mapping configuration)</param>
        /// <returns>(Un)Registered mapping configuration</returns>
        public static MappingConfig Add(MappingConfig config)
        {
            string key = $"{config.MainType} - {config.SourceType}";
            if (config.Mappings.Length < 1)
            {
                _Mappings.TryRemove(key, out _);
            }
            else
            {
                _Mappings[key] = config;
            }
            return config;
        }

        /// <summary>
        /// Get mappings for a source/main object mapping
        /// </summary>
        /// <param name="source">Source type</param>
        /// <param name="main">Main type</param>
        /// <returns>Mappings or <see langword="null"/>, if not found</returns>
        public static MappingConfig? Get(Type source, Type main)
        {
            _Mappings.TryGetValue($"{main} - {source}", out MappingConfig? res);
            return res;
        }

        /// <summary>
        /// Ensure registered source/main object mapping definitions (will create an automatic mapping, if not exists)
        /// </summary>
        /// <param name="source">Source type</param>
        /// <param name="main">Main type</param>
        /// <returns>Mappings (the final <see cref="Mappings.Find(Type, Type)"/> return value)</returns>
        public static MappingConfig EnsureMappings(Type source, Type main) => Find(source, main) is MappingConfig res ? res : Add(source, main);

        /// <summary>
        /// Find an existing mapping
        /// </summary>
        /// <param name="source">Source object type</param>
        /// <param name="main">Main object type</param>
        /// <returns>Mapping or <see langword="null"/>, if not found</returns>
        public static MappingConfig? Find(Type source, Type main)
            => (from config in _Mappings.Values
                where config.SourceType.IsAssignableFrom(source) &&
                config.MainType.IsAssignableFrom(main)
                select config)
            .OrderByDescending(config => config.MainType == main)
            .ThenByDescending(config => config.SourceType == source)
            .FirstOrDefault();

        /// <summary>
        /// Clear all registered mappings
        /// </summary>
        public static void Clear() => _Mappings.Clear();

        /// <summary>
        /// Map a source object to a main object instance
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Main object</returns>
        public static tMain MapFrom<tSource, tMain>(tSource source, tMain main, MappingConfig? config = null)
            where tSource : class
            where tMain : class
        {
            try
            {
                if (main is MappingObjectBase<tSource> mappingObject)
                {
                    mappingObject.MapFrom(source);
                }
                else if (main is IAdapterMappingObject<tSource, tMain> adapterMappingObject)
                {
                    adapterMappingObject.MapFrom(source);
                }
                else
                {
                    config ??= EnsureMappings(source.GetType(), main.GetType());
                    config.BeforeMapping?.Invoke(source, main, config);
                    foreach (Mapping map in config.Mappings) map.MapFrom(source, main);
                    if (main is IMappingObject<tSource> genericMappingObjectType)
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
            catch(Exception ex)
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
        /// <returns>Main object</returns>
        public static object MapFromObject(object source, object main, MappingConfig? config = null)
            => MapFromMethod.MakeGenericMethod(source.GetType(), main.GetType()).Invoke(obj: null, new object?[] { source, main, config })
            ?? throw new MappingException("MapFrom returned NULL");

        /// <summary>
        /// Map a main object to a source object instance (apply reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Source object</returns>
        public static tSource MapTo<tMain, tSource>(tMain main, tSource source, MappingConfig? config = null)
            where tMain : class
            where tSource : class
        {
            try
            {
                if (main is MappingObjectBase<tSource> mappingObject)
                {
                    mappingObject.MapTo(source);
                }
                else if (main is IAdapterMappingObject<tSource, tMain> adapterMappingObject)
                {
                    adapterMappingObject.MapTo(source);
                }
                else
                {
                    config ??= EnsureMappings(source.GetType(), main.GetType());
                    config.BeforeReverseMapping?.Invoke(source, main, config);
                    foreach (Mapping map in config.Mappings) map.MapTo(main, source);
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
        /// <returns>Source object</returns>
        public static object MapToObject(object main, object source, MappingConfig? config = null)
            => MapToMethod.MakeGenericMethod(main.GetType(), source.GetType()).Invoke(obj: null, new object?[] { main, source, config })
            ?? throw new MappingException("MapTo returned NULL");

        /// <summary>
        /// Map a list of source objects to a list of main objects
        /// </summary>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <param name="sources">Source objects</param>
        /// <param name="mainInstanceFactory">Main object instance factory</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Main objects</returns>
        public static IEnumerable<tMain> MapAllFrom<tSource, tMain>(this IEnumerable<tSource> sources, Func<tSource, tMain>? mainInstanceFactory = null, MappingConfig? config = null)
            where tSource : class
            where tMain : class
        {
            mainInstanceFactory ??= (source) => (tMain)(Activator.CreateInstance(typeof(tMain)) ?? throw new MappingException($"Failed to instance {typeof(tMain)}"));
            config ??= EnsureMappings(typeof(tSource), typeof(tMain));
            foreach (tSource source in sources) yield return MapFrom(source, mainInstanceFactory(source), config);
        }
    }
}
