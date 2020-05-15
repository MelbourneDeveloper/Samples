using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MockDelegates
{
    [TestClass]
    public partial class GenericTypeTests
    {
        [TestMethod]
        public void Test2()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IDoer, Doer>();

            serviceCollection.AddSingleton<Do<A>>((sp) => (a) => a);
            serviceCollection.AddSingleton<Do<B>>((sp) => (b) => b);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var doer = serviceProvider.GetService<IDoer>();
            var doA = serviceProvider.GetService<Do<A>>();

            var a = doer.Do(new A());
            var aa = doA(new A());
        }
    }

    public class A
    {
        public string Test { get; set; }
    }

    public class B
    {
        public string Test { get; set; }
    }

    public delegate T Do<T>(T value) where T : class, new();

    public interface IDoer
    {
        T Do<T>(T value) where T : class, new();
    }

    public class Doer : IDoer
    {
        public T Do<T>(T value) where T : class, new()
        {
            if (value is A a) return (T)(object)a;
            if (value is B b) return (T)(object)b;
            throw new NotImplementedException();
        }
    }
}
