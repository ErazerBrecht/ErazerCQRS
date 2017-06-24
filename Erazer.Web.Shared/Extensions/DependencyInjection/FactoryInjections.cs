using System;
using Microsoft.Extensions.DependencyInjection;
using Erazer.Framework.Factories;

namespace Erazer.Shared.Extensions.DependencyInjection
{
    public static class FactoryInjections
    {
        public static IServiceCollection AddSingletonFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddScopedFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Scoped);
        }

        public static IServiceCollection AddTransientFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Transient);
        }

        private static IServiceCollection AddInternal<T, TFactory>(
            this IServiceCollection collection,
            Func<IServiceProvider, TFactory> factoryProvider,
            ServiceLifetime lifetime) where T : class where TFactory : class, IFactory<T>
        {
            Func<IServiceProvider, object> factoryFunc = provider =>
            {
                var factory = factoryProvider(provider);
                return factory.Build();
            };
            var descriptor = new ServiceDescriptor(typeof(T), factoryFunc, lifetime);
            collection.Add(descriptor);
            return collection;
        }
    }
}
