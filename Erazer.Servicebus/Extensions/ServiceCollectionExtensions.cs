using Erazer.Framework.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Erazer.Servicebus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void StartEventReciever(this IServiceCollection collection)
        {
            collection.AddSingleton<IEventReciever, EventReciever>();

            // Build the intermediate service provider
            var sp = collection.BuildServiceProvider();
            var service = sp.GetService<IEventReciever>();

            service.RegisterEventReciever();
        }
    }
}
