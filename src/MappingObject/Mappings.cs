using System.Reflection;

namespace wan24.MappingObject
{
    /// <summary>
    /// Mappings
    /// </summary>
    public static class Mappings
    {
        /// <summary>
        /// Mappings
        /// </summary>
        private static readonly Dictionary<string, Mapping[]> _Mappings = new();

        /// <summary>
        /// Types of registered mappings (the first type is the main type name, the second type is the source type name)
        /// </summary>
        public static IEnumerable<string> Types => _Mappings.Keys;

        /// <summary>
        /// Add a mapping
        /// </summary>
        /// <param name="source">Source type</param>
        /// <param name="main">Main type</param>
        /// <param name="mappings">Mappings</param>
        public static void Add(Type source, Type main, params Mapping[] mappings)
        {
            Dictionary<string, Mapping> maps = new();
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
                maps[mpi.Name] = new(mpi.Name, attr?.Source);
            }
            foreach (Mapping map in mappings) maps[map.MainPropertyName] = map;
            _Mappings[$"{main} - {source}"] = maps.Values.ToArray();
        }

        /// <summary>
        /// Add a mapping (use only the given mappings)
        /// </summary>
        /// <param name="source">Source type</param>
        /// <param name="main">Main type</param>
        /// <param name="mappings">Mappings</param>
        public static void AddExplicit(Type source, Type main, params Mapping[] mappings)
        {
            string key = $"{main} - {source}";
            if (mappings.Length > 0)
            {
                _Mappings[key] = mappings;
            }
            else if (_Mappings.ContainsKey(key))
            {
                _Mappings.Remove(key);
            }
        }

        /// <summary>
        /// Get mappings
        /// </summary>
        /// <param name="source">Source type</param>
        /// <param name="main">Main type</param>
        /// <returns>Mappings or <see langword="null"/>, if not found</returns>
        public static Mapping[]? Get(Type source, Type main)
        {
            string types = $"{main} - {source}";
            return _Mappings.ContainsKey(types) ? _Mappings[types] : null;
        }

        /// <summary>
        /// Ensure a mapping
        /// </summary>
        /// <param name="source">Source type</param>
        /// <param name="main">Main type</param>
        /// <returns>Mappings</returns>
        public static Mapping[] EnsureMapping(Type source, Type main)
        {
            Mapping[]? res = Get(source, main);
            if (res != null) return res;
            Add(source, main);
            return Get(source, main)!;
        }

        /// <summary>
        /// Map a source object to a main object
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        public static void MapFrom<tSource, tMain>(tSource source, tMain main)
            where tSource : class
            where tMain : class
        {
            if (main is MappingObjectBase<tSource> mappingObject)
            {
                mappingObject.MapFrom(source);
            }
            else
            {
                foreach (Mapping map in EnsureMapping(source.GetType(), main.GetType())) map.MapFrom(source, main);
            }
        }

        /// <summary>
        /// Map a main object to a source object (apply reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        public static void MapTo<tMain, tSource>(tMain main, tSource source)
            where tMain : class
            where tSource : class
        {
            if (main is MappingObjectBase<tSource> mappingObject)
            {
                mappingObject.MapTo(source);
            }
            else
            {
                foreach (Mapping map in EnsureMapping(source.GetType(), main.GetType())) map.MapTo(main, source);
            }
        }

        /// <summary>
        /// Map a list of source objects to a list of main objects
        /// </summary>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <param name="sources">Source objects</param>
        /// <param name="mainInstanceFactory">Main object instance factory</param>
        /// <returns>Main objects</returns>
        public static IEnumerable<tMain> MapAllFrom<tSource, tMain>(this IEnumerable<tSource> sources, Func<tSource, tMain>? mainInstanceFactory = null)
            where tSource : class
            where tMain : class
        {
            mainInstanceFactory ??= (source) => (tMain)(Activator.CreateInstance(typeof(tMain)) ?? throw new MappingException($"Failed to instance {typeof(tMain)}"));
            tMain main;
            foreach (tSource source in sources)
            {
                main = mainInstanceFactory(source);
                MapFrom(source, main);
                yield return main;
            }
        }

        /// <summary>
        /// Map a list of main objects to a list of source objects (reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <param name="mainObjects">Main objects</param>
        /// <param name="sourceInstanceFactory">Source object instance factory</param>
        /// <returns>Source objects</returns>
        public static IEnumerable<tSource> MapAllTo<tMain, tSource>(this IEnumerable<tMain> mainObjects, Func<tMain, tSource>? sourceInstanceFactory = null)
            where tMain : class
            where tSource : class
        {
            sourceInstanceFactory ??= (source) => (tSource)(Activator.CreateInstance(typeof(tSource)) ?? throw new MappingException($"Failed to instance {typeof(tSource)}"));
            tSource source;
            foreach (tMain main in mainObjects)
            {
                source = sourceInstanceFactory(main);
                MapTo(main, source);
                yield return source;
            }
        }

        /// <summary>
        /// Map a list of source/main objects
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="pairs">Source/main objects</param>
        /// <returns>Mapped source/main objects</returns>
        public static IEnumerable<KeyValuePair<tSource, tMain>> MapAllFrom<tSource, tMain>(this IEnumerable<KeyValuePair<tSource, tMain>> pairs)
            where tMain : class
            where tSource : class
        {
            foreach (KeyValuePair<tSource,tMain> pair in pairs)
            {
                MapFrom(pair.Key, pair.Value);
                yield return pair;
            }
        }

        /// <summary>
        /// Map a list of main/source objects
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="pairs">Main/source objects</param>
        /// <returns>Mapped main/source objects</returns>
        public static IEnumerable<KeyValuePair<tMain, tSource>> MapAllTo<tMain, tSource>(this IEnumerable<KeyValuePair<tMain, tSource>> pairs)
            where tSource : class
            where tMain : class
        {
            foreach (KeyValuePair<tMain, tSource> pair in pairs)
            {
                MapTo(pair.Key, pair.Value);
                yield return pair;
            }
        }
    }
}
