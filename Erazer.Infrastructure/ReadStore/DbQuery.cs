using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Read.Application.Infrastructure;
using Erazer.Read.Data;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Erazer.Infrastructure.ReadStore
{
    public class DbQuery<T>: IDbQuery<T> where T : class, IProjection
    {
        private readonly IMongoCollection<T> _collection;

        public DbQuery(IMongoDatabase database)
        {
            var type = typeof(T);
            var collectionName = CollectionNameMapping.CollectionNames.ContainsKey(type)
                ? CollectionNameMapping.CollectionNames[type]
                : type.Name;
            
            _collection = database.GetCollection<T>(collectionName);
        }
        
        public Task<T> Single(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default)
        {
            return _collection.AsQueryable().SingleOrDefaultAsync(query, cancellationToken);
        }

        public async Task<IList<T>> Find(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().Where(query).ToListAsync(cancellationToken);
        }

        public async Task<IList<T>> Find(Expression<Func<T, bool>> query, int limit, CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().Where(query).Take(limit).ToListAsync(cancellationToken);
        }

        public async Task<IList<T>> Find(Expression<Func<T, bool>> query, int limit, int skip, CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().Where(query).Skip(skip).Take(limit).ToListAsync(cancellationToken);
        }

        public async Task<IList<T>> All(CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().ToListAsync(cancellationToken);
        }

        public async Task<IList<T>> All(int limit, CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().Take(limit).ToListAsync(cancellationToken);

        }

        public async Task<IList<T>> All(int limit, int skip, CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().Skip(skip).Take(limit).ToListAsync(cancellationToken);
        }
    }
}