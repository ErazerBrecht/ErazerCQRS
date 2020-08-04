using System;
using System.IO;
using System.Threading.Tasks;
using Erazer.Infrastructure.EventStore;
using Erazer.Infrastructure.MongoDb;
using Erazer.Infrastructure.ReadStore;
using Erazer.Migrations.Migrations;
using Erazer.Migrations.Seeding;
using Erazer.Read.Data.Ticket;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Npgsql;

namespace Erazer.Migrations
{
    class Program
    {
        private static IConfigurationRoot _config;

        private static async Task Main(string[] args)
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
            Migrate(scope.ServiceProvider);
            await Seed(scope.ServiceProvider);
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
                .AddMongo(_config.GetSection("MongoDbSettings"), DbCollectionsSetup.ReadStoreConfiguration)
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add Postgres support to FluentMigrator
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

            if (dbExists) return;
            {
                var command = $@"CREATE DATABASE ""{split[1]}""";
                using var cmd = new NpgsqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Migrate the database(s)
        /// </summary>
        private static void Migrate(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
        
        /// <summary>
        /// Seed the database(s)
        /// </summary>
        private static async Task Seed(IServiceProvider serviceProvider)
        {
            var mongoDatabase = serviceProvider.GetRequiredService<IMongoDatabase>();
            var priorityCollection = serviceProvider.GetRequiredService<IMongoCollection<PriorityDto>>();
            var statusCollection = serviceProvider.GetRequiredService<IMongoCollection<StatusDto>>();
            var collections = serviceProvider.GetRequiredService<CollectionNameDictionary>();

            await CollectionSeeder.Seed(mongoDatabase, collections);
            await PrioritySeeder.Seed(priorityCollection);
            await StatusSeeder.Seed(statusCollection);
        }
    }
}