using wan24.MappingObject;

namespace MappingObject_Async_Tests
{
    internal class TestType3 : IAdapterMappingObjectAsync<TestType2, TestType3>
    {
        private readonly MappingObjectAsyncAdapter<TestType2, TestType3> Adapter;

        internal TestType3()
        {
            Adapter = new(this);
        }

        internal TestType3(TestType2 source) : this() => MapFromAsync(source).Wait();

        public bool Mapped { get; set; }

        public void MapFrom(TestType2 source) => MapFromAsync(source).Wait();

        public Task MapFromAsync(TestType2 source, CancellationToken cancellationToken = default) => Adapter.MapFromAsync(source, cancellationToken);

        public void MapTo(TestType2 source) => MapToAsync(source).Wait();

        public Task MapToAsync(TestType2 source, CancellationToken cancellationToken = default) => Adapter.MapToAsync(source, cancellationToken);
    }
}
