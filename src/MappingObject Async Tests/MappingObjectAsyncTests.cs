using wan24.MappingObject;

namespace MappingObject_Async_Tests
{
    [TestClass]
    public class MappingObjectAsyncTests
    {
        public MappingObjectAsyncTests()
        {
            Mappings.Add(typeof(TestType2), typeof(TestType1))
                .WithAsyncMapping(
                    nameof(TestType1.Mapped),
                    async (source, main, ct) =>
                    {
                        await Task.Yield();
                        TestType2 sourceType = (TestType2)source;
                        TestType1 mainType = (TestType1)main;
                        mainType.Mapped = sourceType.Mapped;
                    },
                    async (main, source, ct) =>
                    {
                        await Task.Yield();
                        TestType1 mainType = (TestType1)main;
                        TestType2 sourceType = (TestType2)source;
                        sourceType.Mapped = mainType.Mapped;
                    });
        }

        [TestMethod]
        public async Task Mapping_Test()
        {
            TestType1 main = await AsyncMappings.MapFromAsync(new TestType2(), new TestType1());
            Assert.IsTrue(main.Mapped);
        }

        [TestMethod]
        public async Task Reverse_Mapping_Test()
        {
            TestType2 source = await AsyncMappings.MapToAsync(new TestType1(), new TestType2());
            Assert.IsFalse(source.Mapped);
        }

        [TestMethod]
        public async Task List_Mapping_Test()
        {
            TestType2[] sources = new TestType2[]
            {
                new(),
                new()
            };
            await foreach (TestType1 main in sources.MapAllFromAsync<TestType2, TestType1>())
                Assert.IsTrue(main.Mapped);
            await foreach (TestType1 main in sources.MapAllFromAsync<TestType2, TestType1>((source) => new()))
                Assert.IsTrue(main.Mapped);
            await foreach (KeyValuePair<TestType2, TestType1> pair in new Dictionary<TestType2, TestType1>()
            {
                {new(),new() },
                {new(),new() }
            }.MapAllFromAsync())
                Assert.IsTrue(pair.Value.Mapped);
        }

        [TestMethod]
        public async Task List_Reverse_Mapping_Test()
        {
            TestType1[] mainObjects = new TestType1[]
            {
                new(),
                new()
            };
            await foreach (TestType2 source in mainObjects.MapAllToAsync<TestType1, TestType2>())
                Assert.IsFalse(source.Mapped);
            await foreach (TestType2 source in mainObjects.MapAllToAsync<TestType1, TestType2>((main) => new()))
                Assert.IsFalse(source.Mapped);
            await foreach (KeyValuePair<TestType1, TestType2> pair in new Dictionary<TestType1, TestType2>()
            {
                {new(),new() },
                {new(),new() }
            }.MapAllToAsync())
                Assert.IsFalse(pair.Value.Mapped);
        }

        [TestMethod]
        public async Task Async_List_Mapping_Test()
        {
            await foreach (TestType1 main in GetSourceObjectsAsync().MapAllFromAsync<TestType2, TestType1>())
                Assert.IsTrue(main.Mapped);
            await foreach (TestType1 main in GetSourceObjectsAsync().MapAllFromAsync<TestType2, TestType1>((source) => new()))
                Assert.IsTrue(main.Mapped);
            await foreach (KeyValuePair<TestType2, TestType1> pair in GetMappingPairsAsync().MapAllFromAsync())
                Assert.IsTrue(pair.Value.Mapped);
        }

        [TestMethod]
        public async Task Async_List_Reverse_Mapping_Test()
        {
            await foreach (TestType2 source in GetMainObjectsAsync().MapAllToAsync<TestType1, TestType2>())
                Assert.IsFalse(source.Mapped);
            await foreach (TestType2 source in GetMainObjectsAsync().MapAllToAsync<TestType1, TestType2>((main) => new()))
                Assert.IsFalse(source.Mapped);
            await foreach (KeyValuePair<TestType1, TestType2> pair in GetReverseMappingPairsAsync().MapAllToAsync())
                Assert.IsFalse(pair.Value.Mapped);
        }

        private static async IAsyncEnumerable<TestType1> GetMainObjectsAsync()
        {
            await Task.Yield();
            foreach (TestType1 main in new TestType1[]
            {
                new(),
                new()
            })
                yield return main;
        }

        private static async IAsyncEnumerable<TestType2> GetSourceObjectsAsync()
        {
            await Task.Yield();
            foreach (TestType2 source in new TestType2[]
            {
                new(),
                new()
            })
                yield return source;
        }

        private static async IAsyncEnumerable<KeyValuePair<TestType2,TestType1>> GetMappingPairsAsync()
        {
            await Task.Yield();
            foreach (KeyValuePair<TestType2, TestType1> pair in new Dictionary<TestType2, TestType1>()
            {
                {new(),new() },
                {new(),new() }
            })
                yield return pair;
        }

        private static async IAsyncEnumerable<KeyValuePair<TestType1, TestType2>> GetReverseMappingPairsAsync()
        {
            await Task.Yield();
            foreach (KeyValuePair<TestType1, TestType2> pair in new Dictionary<TestType1, TestType2>()
            {
                {new(),new() },
                {new(),new() }
            })
                yield return pair;
        }
    }
}
