using System;
using Erazer.Domain.Events;
using Erazer.Framework.Factories;
using Marten;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Erazer.DAL.WriteModel
{
    public class EventStoreFactory : IFactory<IDocumentStore>
    {
        private readonly IOptions<EventStoreSettings> _options;
        private readonly ILogger<IDocumentStore> _logger;

        public EventStoreFactory(IOptions<EventStoreSettings> options, ILogger<IDocumentStore> logger)
        {
            _options = options;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(options.Value.ConnectionString))
                throw new ArgumentNullException(options.Value.ConnectionString, "Connection string is required when setting up a connection with a PostgresSQL server for using 'Marten'");

            _logger.LogInformation($"Building a connection to a PostgreSQL server\n\t ConnectionString: {options.Value.ConnectionString}");
        }

        public IDocumentStore Build()
        {
            var documentStore = DocumentStore.For(_ =>
            {
                _.Connection(_options.Value.ConnectionString);

                _.Events.AddEventType(typeof(TicketCommentEvent));
                _.Events.AddEventType(typeof(TicketPriorityEvent));
            });

            // TODO Check if connection was actually succeeded!
            _logger.LogInformation($"Created a succesful connection with the PostgreSQL server\n\t ConnectionString: {_options.Value.ConnectionString}\n\t");

            return documentStore;
        }
    }
}
