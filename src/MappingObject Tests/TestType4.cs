using wan24.MappingObject;

namespace MappingObject_Tests
{
    internal class TestType4 : IMappingObject<TestType2>
    {
        public TestType4() { }

        public TestType4(TestType2 source) => MapFrom(source);

        public bool Skipped { get; set; } = true;

        [SkipMapping]
        public bool NotMapped { get; set; }

        public bool Mapped { get; set; } = true;

        [Map(nameof(TestType2.AlsoMapped2))]
        public bool AlsoMapped { get; set; } = true;

        [SkipMapping]
        public bool Converted { get; set; }

        [SkipMapping]
        public string Case { get; set; } = "type4";

        public void MapFrom(TestType2 source, bool applyDefaultMappings = true)
        {
            if (applyDefaultMappings) Mappings.MapFrom(source, this);
            Converted = bool.Parse(source.Converted);
            Case = source.Case.ToLower();
        }

        public void MapTo(TestType2 source, bool applyDefaultMappings = true)
        {
            if (applyDefaultMappings) Mappings.MapTo(this, source);
            source.Converted = Converted.ToString();
            source.Case = Case.ToUpper();
        }

        public void MapFrom(object source, bool applyDefaultMappings = true) => MapFrom((TestType2)source, applyDefaultMappings);

        public void MapTo(object source, bool applyDefaultMappings = true) => MapTo((TestType2)source, applyDefaultMappings);
    }
}
