using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.DTO;

namespace Erazer.Syncing.Infrastructure
{
    public interface IDbRepository<T> where T : class, IDto
    {
        Task<T> Find(string id, CancellationToken cancellationToken = default);
        Task<T> Find(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        Task Add(T newDocument, CancellationToken cancellationToken = default);
        Task Mutate(T newValue, CancellationToken cancellationToken = default);

        Task Delete(string id, CancellationToken cancellationToken = default);
        Task Delete(T model, CancellationToken cancellationToken = default);
        Task Delete(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    }
}