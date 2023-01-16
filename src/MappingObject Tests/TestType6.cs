namespace MappingObject_Tests
{
    internal class TestType6 : ITestType
    {
        public bool Skipped { get; set; } = true;

        public bool NotMapped { get; set; }

        public bool Mapped { get; set; } = true;

        public bool AlsoMapped { get; set; } = true;

        public bool Converted { get; set; }

        public string Case { get; set; } = "TYPE6";
    }
}
