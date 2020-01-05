using System;
using Erazer.Framework.Factories;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class FactoryInjections
    {
        public static IServiceCollection AddSingletonFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Singleton);
        }
        
        public static IServiceCollection TryAddSingletonFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.TryAddTransient<TFactory>();
            return TryAddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Singleton);
        }

        public static IServiceCollection AddScopedFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Scoped);
        }
        
        public static IServiceCollection TryAddScopedFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.TryAddTransient<TFactory>();
            return TryAddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Scoped);
        }
        
        public static IServiceCollection AddTransientFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.AddTransient<TFactory>();
            return AddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Transient);
        }
        
        public static IServiceCollection TryAddTransientFactory<T, TFactory>(this IServiceCollection collection) where T : class where TFactory : class, IFactory<T>
        {
            collection.TryAddTransient<TFactory>();
            return TryAddInternal<T, TFactory>(collection, p => p.GetRequiredService<TFactory>(), ServiceLifetime.Transient);
        }

        private static IServiceCollection AddInternal<T, TFactory>(
            this IServiceCollection collection,
            Func<IServiceProvider, TFactory> factoryProvider,
            ServiceLifetime lifetime) where T : class where TFactory : class, IFactory<T>
        {
            object FactoryFunc(IServiceProvider provider)
            {
                var factory = factoryProvider(provider);
                return factory.Build();
            }

            var descriptor = new ServiceDescriptor(typeof(T), FactoryFunc, lifetime);
            collection.Add(descriptor);
            return collection;
        }


        private static IServiceCollection TryAddInternal<T, TFactory>(
            this IServiceCollection collection,
            Func<IServiceProvider, TFactory> factoryProvider,
            ServiceLifetime lifetime) where T : class where TFactory : class, IFactory<T>
        {
            object FactoryFunc(IServiceProvider provider)
            {
                var factory = factoryProvider(provider);
                return factory.Build();
            }

            var descriptor = new ServiceDescriptor(typeof(T), FactoryFunc, lifetime);
            collection.TryAdd(descriptor);
            return collection;
        }
    }
}
