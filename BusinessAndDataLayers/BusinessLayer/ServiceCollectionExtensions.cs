using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

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
        //public static IServiceCollection SetSave<T>(this IServiceCollection serviceCollection, SaveAsync<T> save) => serviceCollection.AddSingleton(save);

        public static IBusinessLayer CreateBusinessLayer(this IServiceProvider serviceProvider)
        {
            return new BusinessLayer(serviceProvider);
        }

        public static IServiceCollection SetSave(this IServiceCollection serviceCollection, object target, MethodInfo methodInfo, IEnumerable<Type> knownTypes)
        {
            foreach (var knownType in knownTypes)
            {
                var type = typeof(SaveAsync<>).MakeGenericType(new Type[] { knownType });
                var method = methodInfo.MakeGenericMethod(new Type[] { knownType });
                var @delegate = Delegate.CreateDelegate(type, target, method);
                serviceCollection.AddSingleton(type, @delegate);
            }

            return serviceCollection;
        }


    }
}