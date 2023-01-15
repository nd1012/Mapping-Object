using wan24.MappingObject;

namespace MappingObject_Tests
{
    internal class TestType3 : MappingObjectBase<TestType2>
    {
        public TestType3() : base() { }

        public TestType3(TestType2 source) : base(source) { }

        public bool Skipped { get; set; } = true;

        [SkipMapping]
        public bool NotMapped { get; set; }

        public bool Mapped { get; set; } = true;

        [Map(nameof(TestType2.AlsoMapped2))]
        public bool AlsoMapped { get; set; } = true;

        [SkipMapping]
        public bool Converted { get; set; }

        [SkipMapping]
        public string Case { get; set; } = "type3";

        public override void MapFrom(TestType2 source)
        {
            base.MapFrom(source);
            Converted = bool.Parse(source.Converted);
            Case = source.Case.ToLower();
        }

        public override void MapTo(TestType2 source)
        {
            base.MapTo(source);
            source.Converted = Converted.ToString();
            source.Case = Case.ToUpper();
        }
    }
}
