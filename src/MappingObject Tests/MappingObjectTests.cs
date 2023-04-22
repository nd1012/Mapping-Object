using wan24.MappingObject;

namespace MappingObject_Tests
{
    [TestClass]
    public class MappingObjectTests
    {
        public MappingObjectTests()
        {
            // Force value converter and getter tests
            Mappings.Add(typeof(TestType2), typeof(TestType1))
                .ConfigureMapping(nameof(TestType1.Converted), (c, m) =>
                {
                    m.WithSourceConverter(v => bool.Parse((string)v!))
                        .WithMainConverter(v => v!.ToString());
                })
                .ConfigureMapping(nameof(TestType1.Case), (c, m) =>
                {
                    m.WithSourceGetter((source, main) => ((TestType2)source).Case.ToLower())
                        .WithMainGetter((main, source) => ((TestType1)main).Case.ToUpper());
                });
            // Force mapper tests
            Mappings.Add(typeof(TestType2), typeof(TestType3))
                .WithMapping(nameof(TestType3.Converted), sourcePropertyName: null, mapping: (c, m) =>
                {
                    m.WithSourceConverter(v => bool.Parse((string)v!))
                        .WithMainConverter(v => v!.ToString());
                })
                .WithMapping(
                    nameof(TestType3.Case),
                    (source, main) =>
                    {
                        TestType2 sourceType = (TestType2)source;
                        TestType3 mainType = (TestType3)main;
                        mainType.Case = sourceType.Case.ToLower();
                    },
                    (main, source) =>
                    {
                        TestType3 mainType = (TestType3)main;
                        TestType2 sourceType = (TestType2)source;
                        sourceType.Case = mainType.Case.ToUpper();
                    });
            // Force abstract type tests
            Mappings.Add(typeof(TestType2), typeof(ITestType))
                .ConfigureMapping(nameof(TestType1.Converted), (c, m) =>
                {
                    m.WithSourceConverter(v => bool.Parse((string)v!))
                        .WithMainConverter(v => v!.ToString());
                })
                .ConfigureMapping(nameof(TestType1.Case), (c, m) =>
                {
                    m.WithSourceGetter((source, main) => ((TestType2)source).Case.ToUpper())
                        .WithMainGetter((main, source) => ((ITestType)main).Case.ToLower());
                });
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

        [TestMethod]
        public void Casting_Test()
        {
            TestType5 main = (TestType5)new TestType2();
            Assert.IsTrue(main.Skipped);
            Assert.IsFalse(main.NotMapped);
            Assert.IsFalse(main.Mapped);
            Assert.IsFalse(main.AlsoMapped);
            Assert.IsTrue(main.Converted);
            Assert.AreEqual("type2", main.Case);
        }

        [TestMethod]
        public void Abstract_Mapping_Test()
        {
            ITestType main = new TestType6();
            Mappings.MapFrom(new TestType2(), main);
            Assert.IsTrue(main.Skipped);
            Assert.IsFalse(main.NotMapped);
            Assert.IsFalse(main.Mapped);
            Assert.IsFalse(main.AlsoMapped);
            Assert.IsTrue(main.Converted);
            Assert.AreEqual("TYPE2", main.Case);
        }

        [TestMethod]
        public void Abstract_Reverse_Mapping_Test()
        {
            TestType2 source = new();
            Mappings.MapTo(new TestType6(), source);
            Assert.IsFalse(source.Skipped2);
            Assert.IsTrue(source.NotMapped);
            Assert.IsTrue(source.Mapped);
            Assert.IsTrue(source.AlsoMapped2);
            Assert.AreEqual(false.ToString(), source.Converted);
            Assert.AreEqual("type6", source.Case);
        }

        [TestMethod]
        public void Handler_Test()
        {
            int beforeMapping = 0,
                afterMapping = 0,
                beforeReverseMapping = 0,
                afterReverseMapping = 0;
            MappingConfig config = (MappingConfig)Mappings.Get(typeof(TestType2), typeof(TestType1))!.Clone();
            config.BeforeMapping = (source, main, c) => beforeMapping++;
            config.AfterMapping = (source, main, c) => afterMapping++;
            config.BeforeReverseMapping = (source, main, c) => beforeReverseMapping++;
            config.AfterReverseMapping = (source, main, c) => afterReverseMapping++;
            TestType1 main = new();
            TestType2 source = new();
            Mappings.MapFrom(source, main, config);
            Mappings.MapTo(main, source, config);
            Assert.AreEqual(1, beforeMapping);
            Assert.AreEqual(1, afterMapping);
            Assert.AreEqual(1, beforeReverseMapping);
            Assert.AreEqual(1, afterReverseMapping);
        }

        [TestMethod]
        public void Nesting_Test()
        {
            TestType7 main = Mappings.MapFrom(new TestType8(), new TestType7());
            Assert.IsNotNull(main.Value1);
        }
    }
}