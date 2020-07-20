using DomainLib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessAndDataLayers
{
    public class DummyPersonAsObjectAsyncEnumerator : IAsyncEnumerator<object>
    {
        private bool _isFirst = true;

        public DummyPersonAsObjectAsyncEnumerator(bool returnAPerson)
        {
            _isFirst = returnAPerson;
        }

        public object Current { get; } = new Person { Name = "Bob" };

        public ValueTask DisposeAsync()
        {
            return default;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            if (_isFirst)
            {
                _isFirst = false;
                return new ValueTask<bool>(true);
            }
            return new ValueTask<bool>(false);
        }
    }
}
