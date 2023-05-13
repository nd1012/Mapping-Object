using wan24.MappingObject;

namespace MappingObject_Tests
{
    internal class TestType9 : TestType1, IAdapterMappingObject<TestType2, TestType9>
    {
        private readonly MappingObjectAdapter<TestType2, TestType9> Adapter;

        internal TestType9()
        {
            Adapter = new(this);
            Case = "type9";
        }

        internal TestType9(TestType2 source) : this() => MapFrom(source);

        public void MapFrom(TestType2 source) => Adapter.MapFrom(source);

        public void MapTo(TestType2 source) => Adapter.MapTo(source);
    }
}
