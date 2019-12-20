using System;
using Erazer.Infrastructure.EventStore;
using FluentMigrator;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Erazer.Migrations.Migrations
{
    [Migration(1576848500)]
    public class CreateEventStoreDatabase : Migration
    {
        private readonly MigrationSettings _migrationOptions;
        private readonly EventStoreSettings _eventStoreSettings;

        public CreateEventStoreDatabase(IOptions<MigrationSettings> migrationOptions, IOptions<EventStoreSettings> eventStoreOptions)
        {
            _migrationOptions = migrationOptions?.Value ?? throw new ArgumentNullException(nameof(migrationOptions));
            _eventStoreSettings = eventStoreOptions?.Value ?? throw new ArgumentNullException(nameof(eventStoreOptions));
        }
        
        public override void Up()
        {
            var connString = _migrationOptions.ConnectionString;
            var databaseName = _eventStoreSettings.ConnectionString.Split("Database=")[1];
            using var conn = new NpgsqlConnection(connString);
            conn.Open();

            using var cmd = new NpgsqlCommand($@"CREATE DATABASE ""{databaseName}""", conn);
            cmd.ExecuteNonQuery();
        }

        public override void Down()
        {
            // TODO: Delete the EventStore database
        }
    }
}