using DomainLib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
{
    public class DummyPersonAsObjectAsyncEnumerator : IAsyncEnumerator<Person>
    {
        private bool _isFirst;

        public DummyPersonAsObjectAsyncEnumerator(bool returnAPerson)
        {
            _isFirst = returnAPerson;
        }

        public Person Current { get; } = new Person { Name = "Bob" };

        public ValueTask DisposeAsync()
        {
            return default;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            if (!_isFirst) return new ValueTask<bool>(false);

            _isFirst = false;
            return new ValueTask<bool>(true);
        }
    }
}
