using Microsoft.Extensions.DependencyInjection;
using System;

namespace BusinessLayerLib
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection OnSaving<T>(this IServiceCollection serviceCollection, Saving<T> saving) => serviceCollection.AddSingleton(saving);
        public static IServiceCollection OnSaved<T>(this IServiceCollection serviceCollection, Saved<T> saved) => serviceCollection.AddSingleton(saved);
        public static IServiceCollection OnDeleting<T>(this IServiceCollection serviceCollection, Deleting<T> deleting) => serviceCollection.AddSingleton(deleting);
        public static IServiceCollection OnDeleted<T>(this IServiceCollection serviceCollection, Deleted<T> deleted) => serviceCollection.AddSingleton(deleted);
        public static IServiceCollection OnFetching<T>(this IServiceCollection serviceCollection, BeforeGet<T> beforeGet) => serviceCollection.AddSingleton(beforeGet);
        public static IServiceCollection OnFetched<T>(this IServiceCollection serviceCollection, AfterGet<T> afterGet) => serviceCollection.AddSingleton(afterGet);

        public static IServiceCollection SetWhere<T>(this IServiceCollection serviceCollection, WhereAsync<T> where) => serviceCollection.AddSingleton(where);
        public static IServiceCollection SetSave<T>(this IServiceCollection serviceCollection, SaveAsync<T> save) => serviceCollection.AddSingleton(save);

        public static IBusinessLayer CreateBusinessLayer(this IServiceProvider serviceProvider)
        {
            return new BusinessLayer(serviceProvider);
        }

    }
}