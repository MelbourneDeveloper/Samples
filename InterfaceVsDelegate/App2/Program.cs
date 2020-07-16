using Abstract2;
using Concrete2;
using Microsoft.Extensions.DependencyInjection;

namespace App2
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<Service>();
            serviceCollection.AddSingleton<Execute>((sp)=> sp.GetRequiredService<Service>().Execute);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var execute = serviceProvider.GetRequiredService<Execute>();
            execute();
        }
    }
}
