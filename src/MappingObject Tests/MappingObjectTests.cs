using wan24.MappingObject;

namespace MappingObject_Tests
{
    [TestClass]
    public class MappingObjectTests
    {
        public MappingObjectTests()
        {
            Mappings.Add(
                typeof(TestType2),
                typeof(TestType1),
                new Mapping(nameof(TestType1.Converted), (v) => bool.Parse((string)v!), (v) => v!.ToString()),
                new Mapping(nameof(TestType1.Case), sourceGetter: (source, main) => ((TestType2)source).Case.ToLower(), mainGetter: (main, source) => ((TestType1)main).Case.ToUpper())
                );
        }

        [TestMethod]
        public void Mapping_Test()
        {
            TestType1 main = new();
            Mappings.MapFrom(new TestType2(), main);
            Assert.IsTrue(main.Skipped);
            Assert.IsFalse(main.NotMapped);
            Assert.IsFalse(main.Mapped);
            Assert.IsFalse(main.AlsoMapped);
            Assert.IsTrue(main.Converted);
            Assert.AreEqual("type2", main.Case);
        }

        [TestMethod]
        public void Reverse_Mapping_Test()
        {
            TestType2 source = new();
            Mappings.MapTo(new TestType1(), source);
            Assert.IsFalse(source.Skipped2);
            Assert.IsTrue(source.NotMapped);
            Assert.IsTrue(source.Mapped);
            Assert.IsTrue(source.AlsoMapped2);
            Assert.AreEqual(false.ToString(), source.Converted);
            Assert.AreEqual("TYPE1", source.Case);
        }

        [TestMethod]
        public void List_Mapping_Test()
        {
            TestType2[] sources = new TestType2[]
            {
                new(),
                new()
            };
            foreach (TestType1 main in sources.MapAllFrom<TestType2, TestType1>())
                Assert.IsFalse(main.Mapped);
            foreach (TestType1 main in sources.MapAllFrom<TestType2, TestType1>((source) => new()))
                Assert.IsFalse(main.Mapped);

            foreach (KeyValuePair<TestType2, TestType1> pair in new Dictionary<TestType2, TestType1>()
            {
                {new(),new() },
                {new(),new() }
            }.MapAllFrom())
                Assert.IsFalse(pair.Value.Mapped);
        }

        [TestMethod]
        public void List_Reverse_Mapping_Test()
        {
            TestType1[] mainObjects = new TestType1[]
            {
                new(),
                new()
            };
            foreach (TestType2 source in mainObjects.MapAllTo<TestType1, TestType2>())
                Assert.IsTrue(source.Mapped);
            foreach (TestType2 source in mainObjects.MapAllTo<TestType1, TestType2>((main) => new()))
                Assert.IsTrue(source.Mapped);

            foreach (KeyValuePair<TestType1, TestType2> pair in new Dictionary<TestType1, TestType2>()
            {
                {new(),new() },
                {new(),new() }
            }.MapAllTo())
                Assert.IsTrue(pair.Value.Mapped);
        }

        [TestMethod]
        public void MappingObject_Test()
        {
            TestType3 main = new();
            Mappings.MapFrom(new TestType2(), main);
            Assert.IsTrue(main.Skipped);
            Assert.IsFalse(main.NotMapped);
            Assert.IsFalse(main.Mapped);
            Assert.IsFalse(main.AlsoMapped);
            Assert.IsTrue(main.Converted);
            Assert.AreEqual("type2", main.Case);
        }

        [TestMethod]
        public void Reverse_MappingObject_Test()
        {
            TestType2 source = new();
            Mappings.MapTo(new TestType3(), source);
            Assert.IsFalse(source.Skipped2);
            Assert.IsTrue(source.NotMapped);
            Assert.IsTrue(source.Mapped);
            Assert.IsTrue(source.AlsoMapped2);
            Assert.AreEqual(false.ToString(), source.Converted);
            Assert.AreEqual("TYPE3", source.Case);
        }

        [TestMethod]
        public void IMappingObject_Test()
        {
            TestType4 main = new();
            Mappings.MapFrom(new TestType2(), main);
            Assert.IsTrue(main.Skipped);
            Assert.IsFalse(main.NotMapped);
            Assert.IsFalse(main.Mapped);
            Assert.IsFalse(main.AlsoMapped);
            Assert.IsTrue(main.Converted);
            Assert.AreEqual("type2", main.Case);
        }

        [TestMethod]
        public void Reverse_IMappingObject_Test()
        {
            TestType2 source = new();
            Mappings.MapTo(new TestType4(), source);
            Assert.IsFalse(source.Skipped2);
            Assert.IsTrue(source.NotMapped);
            Assert.IsTrue(source.Mapped);
            Assert.IsTrue(source.AlsoMapped2);
            Assert.AreEqual(false.ToString(), source.Converted);
            Assert.AreEqual("TYPE4", source.Case);
        }

        [TestMethod]
        public void IMappingObject_Direct_Test()
        {
            TestType4 main = new();
            main.MapFrom(new TestType2());
            Assert.IsTrue(main.Skipped);
            Assert.IsFalse(main.NotMapped);
            Assert.IsFalse(main.Mapped);
            Assert.IsFalse(main.AlsoMapped);
            Assert.IsTrue(main.Converted);
            Assert.AreEqual("type2", main.Case);
        }

        [TestMethod]
        public void Reverse_IMappingObject_Direct_Test()
        {
            TestType2 source = new();
            new TestType4().MapTo(source);
            Assert.IsFalse(source.Skipped2);
            Assert.IsTrue(source.NotMapped);
            Assert.IsTrue(source.Mapped);
            Assert.IsTrue(source.AlsoMapped2);
            Assert.AreEqual(false.ToString(), source.Converted);
            Assert.AreEqual("TYPE4", source.Case);
        }
    }
}