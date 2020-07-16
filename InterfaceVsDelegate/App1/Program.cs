using Abstract1;
using Concrete1;
using Microsoft.Extensions.DependencyInjection;

namespace App1
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IService, Service>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var service = serviceProvider.GetRequiredService<IService>();
            service.Execute();
        }
    }
}
