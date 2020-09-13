using Microsoft.Extensions.DependencyInjection;

namespace BusinessLayerLib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection OnSaving<T>(this IServiceCollection serviceCollection, Saving<T> saving) => serviceCollection.AddSingleton(saving);

    }
}