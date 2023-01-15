using wan24.MappingObject;

namespace MappingObject_Tests
{
    internal class TestType1
    {
        public bool Skipped { get; set; } = true;

        [SkipMapping]
        public bool NotMapped { get; set; }

        public bool Mapped { get; set; } = true;

        [Map(nameof(TestType2.AlsoMapped2))]
        public bool AlsoMapped { get; set; } = true;

        public bool Converted { get; set; }

        public string Case { get; set; } = "type1";
    }
}
