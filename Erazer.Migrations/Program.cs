using Erazer.Migrations.Migrations;

namespace Erazer.Migrations
{
    class Program
    {
        private static IConfigurationRoot _config;

        private static void Main(string[] args)
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceProvider = CreateServices();

            // Create migrations db if it not exists yet
            CreateMigrationsDatabase();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using var scope = serviceProvider.CreateScope();
            UpdateDatabase(scope.ServiceProvider);
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </summary>
        private static IServiceProvider CreateServices()
        {
            var connectionString = _config.GetSection("MigrationSettings")["ConnectionString"];

            return new ServiceCollection()
                .Configure<MigrationSettings>(_config.GetSection("MigrationSettings"))
                .Configure<EventStoreSettings>(_config.GetSection("EventStoreSettings"))
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(CreateEventStoreDatabase).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        }

        private static void CreateMigrationsDatabase()
        {
            var connectionString = _config.GetSection("MigrationSettings")["ConnectionString"];
            var split = connectionString.Split("Database=");

            using var conn = new NpgsqlConnection(split[0]);
            
            bool dbExists;
            conn.Open();

            using (var cmd = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname='{split[1]}'", conn))
                dbExists = cmd.ExecuteScalar() != null;

            if (!dbExists)
            {
                var command = $@"CREATE DATABASE ""{split[1]}""";
                using var cmd = new NpgsqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }
}