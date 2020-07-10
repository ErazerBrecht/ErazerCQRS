using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.DTO;
using Erazer.Infrastructure.MongoDb;
using Erazer.Syncing.Infrastructure;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore
{
    public class DbBatchRepository<T> : IDbRepository<T> where T : class, IDto
    {
        private readonly IMongoCollection<T> _collection;

        private List<WriteModel<T>> _bulk;
        private Dictionary<string, T> _memory;

        public DbBatchRepository(IMongoDatabase db)
        {
            if (db == null) throw new ArgumentNullException(nameof(db));

            var factory = new MongoDbCollectionFactory<T>(db);
            _collection = factory.Build();

            _bulk = new List<WriteModel<T>>();
            _memory = new Dictionary<string, T>();
        }

        public async Task<T> Find(string id, CancellationToken cancellationToken = default)
        {
            var exists = _memory.ContainsKey(id);
            if (exists) return _memory[id];

            var result = await _collection.Find(x => x.Id == id).SingleOrDefaultAsync(cancellationToken);

            if (result != null)
                _memory.Add(result.Id, result);

            return result;
        }

        public async Task<T> Find(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            var func = filter.Compile();
            var result = _memory.Values.SingleOrDefault(func);
            if (result != null) return result;

            result = await _collection.Find(filter).SingleOrDefaultAsync(cancellationToken);

            if (result != null)
                _memory.Add(result.Id, result);

            return result;
        }

        public Task Add(T newDocument, CancellationToken cancellationToken = default)
        {
            _memory.Add(newDocument.Id, newDocument);

            _bulk.Add(new InsertOneModel<T>(newDocument));
            return Task.CompletedTask;
        }

        public Task Mutate(T newValue, CancellationToken cancellationToken = default)
        {
            if (_memory.ContainsKey(newValue.Id))
                _memory[newValue.Id] = newValue;
            else
                _memory.Add(newValue.Id, newValue);

            var filter = new ExpressionFilterDefinition<T>(x => x.Id == newValue.Id);
            _bulk.Add(new ReplaceOneModel<T>(filter, newValue) {IsUpsert = true});

            return Task.CompletedTask;
        }

        public Task Delete(string id, CancellationToken cancellationToken = default)
        {
            if (_memory.ContainsKey(id))
                _memory[id] = null;
            else
                _memory.Add(id, null);

            _bulk.Add(new DeleteOneModel<T>(new ExpressionFilterDefinition<T>(x => x.Id == id)));
            return Task.CompletedTask;
        }

        public Task Delete(T model, CancellationToken cancellationToken = default)
        {
            return Delete(model.Id, cancellationToken: cancellationToken);
        }

        public Task Delete(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            // TODO FIX MEMORY PART HERE!!!
            // TODO IT'S GETTING LATE

            _bulk.Add(new DeleteManyModel<T>(filter));
            return Task.CompletedTask;
        }

        public Task Flush(IDbSession dbSession)
        {
            var bulk = _bulk.ToList();

            _bulk = new List<WriteModel<T>>();
            _memory = new Dictionary<string, T>();

            return bulk.Any()
                ? _collection.BulkWriteAsync(dbSession.Handle, bulk)
                : Task.CompletedTask;
        }
    }
}