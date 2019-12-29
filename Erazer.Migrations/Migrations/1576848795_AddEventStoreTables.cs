namespace Erazer.Migrations.Migrations
{
    [Migration(1576848795)]
    public class AddEventStoreTables : Migration
    {
        private readonly EventStoreSettings _options;

        public AddEventStoreTables(IOptions<EventStoreSettings> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        public override void Up()
        {
            var settings = new PostgresStreamStoreSettings(_options.ConnectionString);

            using var streamStore = new PostgresStreamStore(settings);
            streamStore.CreateSchemaIfNotExists().Wait();
        }

        public override void Down()
        {
            // TODO: Remove the EventStore tables
        }
    }
}