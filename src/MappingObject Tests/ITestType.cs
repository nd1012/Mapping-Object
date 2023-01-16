using wan24.MappingObject;

namespace MappingObject_Tests
{
    internal interface ITestType
    {
        bool Skipped { get; set; }
        [SkipMapping]
        bool NotMapped { get; set; }
        bool Mapped { get; set; }
        [Map(nameof(TestType2.AlsoMapped2))]
        bool AlsoMapped { get; set; }
        bool Converted { get; set; }
        string Case { get; set; }
    }
}
