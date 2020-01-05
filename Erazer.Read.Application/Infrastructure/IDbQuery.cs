using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.DTO;
using Erazer.Read.Data;

namespace Erazer.Read.Application.Infrastructure
{
    public interface IDbQuery<T> where T : class, IDto
    {
        Task<T> Single(string id, CancellationToken cancellationToken = default);
        Task<T> Single(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default);
        
        Task<IList<T>> Find(Expression<Func<T, bool>> query, CancellationToken cancellationToken = default);
        Task<IList<T>> Find(Expression<Func<T, bool>> query, int limit, CancellationToken cancellationToken = default);
        Task<IList<T>> Find(Expression<Func<T, bool>> query, int limit, int skip, CancellationToken cancellationToken = default);
        
        Task<IList<T>> All(CancellationToken cancellationToken = default);
        Task<IList<T>> All(int limit, CancellationToken cancellationToken = default);
        Task<IList<T>> All(int limit, int skip, CancellationToken cancellationToken = default);
    }
}