using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.DTO;
using Erazer.Infrastructure.MongoDb;
using Erazer.Read.Data;
using Erazer.Syncing.Infrastructure;
using MongoDB.Driver;

namespace Erazer.Infrastructure.ReadStore
{
    public class DbHelper<T>: IDbHelper<T> where T : class, IDto, new()
    {
        private readonly IDbSession _session;
        private readonly IMongoCollection<T> _collection;

        public DbHelper(IMongoCollection<T> collection, IDbSession session)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
            _session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public Task<T> Find(string id, CancellationToken cancellationToken = default)
        {
            return Find(x => x.Id == id, cancellationToken);
        }

        public Task<T> Find(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            return _collection.Find(_session.Handle, filter).SingleOrDefaultAsync(cancellationToken);
        }

        public Task Add(T mutatedDocument, CancellationToken cancellationToken = default)
        {
            return _collection.InsertOneAsync(_session.Handle, mutatedDocument, cancellationToken: cancellationToken);
        }

        public Task Set<TField>(string id, Expression<Func<T, TField>> field, TField value, CancellationToken cancellationToken = default)
        {
            return Set(x => x.Id == id, field, value, cancellationToken);
        }

        public Task Set<TField>(Expression<Func<T, bool>> filter, Expression<Func<T, TField>> field, TField value, CancellationToken cancellationToken = default)
        {
            var updateDefinition = Builders<T>.Update.Set(field, value);
            return _collection.UpdateOneAsync(_session.Handle, filter, updateDefinition, cancellationToken: cancellationToken);
        }

        public Task Mutate(T projection, CancellationToken cancellationToken = default)
        {
            return _collection.ReplaceOneAsync(_session.Handle, x => x.Id == projection.Id, projection, cancellationToken: cancellationToken);
        }

        public Task Mutate(string id,  Action<T> mutator, CancellationToken cancellationToken = default)
        {
            return Mutate(x => x.Id == id, mutator, cancellationToken);
        }
        
        public Task Mutate(Expression<Func<T, bool>> filter, Action<T> mutator, CancellationToken cancellationToken = default)
        {
            var mutatedDocument = new T();
            mutator(mutatedDocument);
            return _collection.ReplaceOneAsync(_session.Handle, filter, mutatedDocument, cancellationToken: cancellationToken);
        }

        public Task Delete(string id, CancellationToken cancellationToken = default)
        {
            return Delete(x => x.Id == id, cancellationToken);
        }

        public Task Delete(T projection, CancellationToken cancellationToken = default)
        {
            return Delete(x => x.Id == projection.Id, cancellationToken: cancellationToken);
        }

        public Task Delete(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
        {
            return _collection.DeleteOneAsync(_session.Handle, filter, cancellationToken: cancellationToken);
        }
    }
}