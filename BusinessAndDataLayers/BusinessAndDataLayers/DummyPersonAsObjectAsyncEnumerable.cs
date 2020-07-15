using System.Collections.Generic;
using System.Threading;

namespace BusinessAndDataLayers
{
    public class DummyPersonAsObjectAsyncEnumerable : IAsyncEnumerable<object>
    {
        DummyPersonAsObjectAsyncEnumerator dummyPersonAsObjectAsyncEnumerator;

        public DummyPersonAsObjectAsyncEnumerable(bool returnAPerson)
        {
            dummyPersonAsObjectAsyncEnumerator = new DummyPersonAsObjectAsyncEnumerator(returnAPerson);
        }

        public IAsyncEnumerator<object> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return dummyPersonAsObjectAsyncEnumerator;
        }
    }
}
