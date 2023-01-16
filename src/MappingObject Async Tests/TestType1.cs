using wan24.MappingObject;

namespace MappingObject_Async_Tests
{
    internal class TestType1 : MappingObjectAsyncBase<TestType2>
    {
        public bool Mapped { get; set; }
    }
}
