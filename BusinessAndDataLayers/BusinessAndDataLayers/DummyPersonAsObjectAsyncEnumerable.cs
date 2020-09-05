using DomainLib;
using System.Collections.Generic;
using System.Threading;

namespace BusinessAndDataLayers
{
    public class DummyPersonAsObjectAsyncEnumerable : IAsyncEnumerable<Person>
    {
        private readonly DummyPersonAsObjectAsyncEnumerator dummyPersonAsObjectAsyncEnumerator;

        public DummyPersonAsObjectAsyncEnumerable(bool returnAPerson)
        {
            dummyPersonAsObjectAsyncEnumerator = new DummyPersonAsObjectAsyncEnumerator(returnAPerson);
        }

        public IAsyncEnumerator<Person> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return dummyPersonAsObjectAsyncEnumerator;
        }
    }
}
