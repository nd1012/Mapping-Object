using wan24.MappingObject;

namespace MappingObject_Tests
{
    internal sealed class TestType5 : MappingObjectCastableBase<TestType2, TestType5>
    {
        public TestType5() : base() { }

        public TestType5(TestType2 source) : base(source) { }

        public bool Skipped { get; set; } = true;

        [SkipMapping]
        public bool NotMapped { get; set; }

        public bool Mapped { get; set; } = true;

        [Map(nameof(TestType2.AlsoMapped2))]
        public bool AlsoMapped { get; set; } = true;

        [SkipMapping]
        public bool Converted { get; set; }

        [SkipMapping]
        public string Case { get; set; } = "type5";

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
