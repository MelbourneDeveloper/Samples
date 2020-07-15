using System.Collections.Generic;
using System.Threading;

namespace BusinessAndDataLayers
{
    public class DummyPersonAsObjectAsyncEnumerable : IAsyncEnumerable<object>
    {
        public IAsyncEnumerator<object> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new DummyPersonAsObjectAsyncEnumerator();
        }
    }
}
