// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public class EventStoreBuilder
    {
        public IServiceCollection Collection { get; }
     
        public EventStoreBuilder(IServiceCollection serviceCollection)
        {
            Collection = serviceCollection;
        }
    }
}