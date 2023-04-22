namespace wan24.MappingObject
{
    /// <summary>
    /// Mapping extensions
    /// </summary>
    public static class MappingExtensions
    {
        /// <summary>
        /// Map a source object to a main object instance
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="main">Main object</param>
        /// <param name="source">Source object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Main object</returns>
        public static tMain MapFrom<tSource, tMain>(this tMain main, tSource source, MappingConfig? config = null)
            where tSource : class
            where tMain : class
            => Mappings.MapFrom(source, main, config);

        /// <summary>
        /// Map a main object to a source object instance (apply reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="source">Source object</param>
        /// <param name="main">Main object</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Source object</returns>
        public static tSource MapTo<tMain, tSource>(this tSource source, tMain main, MappingConfig? config = null)
            where tMain : class
            where tSource : class
            => Mappings.MapTo(main, source, config);

        /// <summary>
        /// Map a list of source objects to a list of main objects
        /// </summary>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <param name="sources">Source objects</param>
        /// <param name="mainInstanceFactory">Main object instance factory</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Main objects</returns>
        public static IEnumerable<tMain> MapAllFrom<tSource, tMain>(this IQueryable<tSource> sources, Func<tSource, tMain>? mainInstanceFactory = null, MappingConfig? config = null)
            where tSource : class
            where tMain : class
            => sources.AsEnumerable().MapAllFrom(mainInstanceFactory, config);

        /// <summary>
        /// Map a list of main objects to a list of source objects (reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <param name="mainObjects">Main objects</param>
        /// <param name="sourceInstanceFactory">Source object instance factory</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Source objects</returns>
        public static IEnumerable<tSource> MapAllTo<tMain, tSource>(this IEnumerable<tMain> mainObjects, Func<tMain, tSource>? sourceInstanceFactory = null, MappingConfig? config = null)
            where tMain : class
            where tSource : class
        {
            sourceInstanceFactory ??= (source) => (tSource)(Activator.CreateInstance(typeof(tSource)) ?? throw new MappingException($"Failed to instance {typeof(tSource)}"));
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            foreach (tMain main in mainObjects) yield return Mappings.MapTo(main, sourceInstanceFactory(main), config);
        }

        /// <summary>
        /// Map a list of main objects to a list of source objects (reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main type</typeparam>
        /// <typeparam name="tSource">Source type</typeparam>
        /// <param name="mainObjects">Main objects</param>
        /// <param name="sourceInstanceFactory">Source object instance factory</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Source objects</returns>
        public static IEnumerable<tSource> MapAllTo<tMain, tSource>(this IQueryable<tMain> mainObjects, Func<tMain, tSource>? sourceInstanceFactory = null, MappingConfig? config = null)
            where tMain : class
            where tSource : class
            => mainObjects.AsEnumerable().MapAllTo(sourceInstanceFactory, config);

        /// <summary>
        /// Map a list of source/main objects
        /// </summary>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <param name="pairs">Source/main objects</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Mapped source/main objects</returns>
        public static IEnumerable<KeyValuePair<tSource, tMain>> MapAllFrom<tSource, tMain>(this IEnumerable<KeyValuePair<tSource, tMain>> pairs, MappingConfig? config = null)
            where tMain : class
            where tSource : class
        {
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            foreach (KeyValuePair<tSource, tMain> pair in pairs)
            {
                Mappings.MapFrom(pair.Key, pair.Value, config);
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
        /// <returns>Mapped source/main objects</returns>
        public static IEnumerable<KeyValuePair<tSource, tMain>> MapAllFrom<tSource, tMain>(this IQueryable<KeyValuePair<tSource, tMain>> pairs, MappingConfig? config = null)
            where tMain : class
            where tSource : class
            => pairs.AsEnumerable().MapAllFrom(config);

        /// <summary>
        /// Map a list of main/source objects (reverse mapping)
        /// </summary>
        /// <typeparam name="tMain">Main object type</typeparam>
        /// <typeparam name="tSource">Source object type</typeparam>
        /// <param name="pairs">Main/source objects</param>
        /// <param name="config">Default mapping configuration to use</param>
        /// <returns>Mapped main/source objects</returns>
        public static IEnumerable<KeyValuePair<tMain, tSource>> MapAllTo<tMain, tSource>(this IEnumerable<KeyValuePair<tMain, tSource>> pairs, MappingConfig? config = null)
            where tSource : class
            where tMain : class
        {
            config ??= Mappings.EnsureMappings(typeof(tSource), typeof(tMain));
            foreach (KeyValuePair<tMain, tSource> pair in pairs)
            {
                Mappings.MapTo(pair.Key, pair.Value, config);
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
        /// <returns>Mapped main/source objects</returns>
        public static IEnumerable<KeyValuePair<tMain, tSource>> MapAllTo<tMain, tSource>(this IQueryable<KeyValuePair<tMain, tSource>> pairs, MappingConfig? config = null)
            where tSource : class
            where tMain : class
            => pairs.AsEnumerable().MapAllTo(config);
    }
}
