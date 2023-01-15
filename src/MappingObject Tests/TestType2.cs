namespace MappingObject_Tests
{
    internal class TestType2
    {
        public bool Skipped2 { get; set; }

        public bool NotMapped { get; set; } = true;

        public bool Mapped { get; set; }

        public bool AlsoMapped2 { get; set; }

        public string Converted { get; set; } = true.ToString();

        public string Case { get; set; } = "TYPE2";
    }
}
