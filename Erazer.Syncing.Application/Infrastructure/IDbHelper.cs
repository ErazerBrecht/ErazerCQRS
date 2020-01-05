using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Erazer.Framework.DTO;
using Erazer.Read.Data;

namespace Erazer.Syncing.Infrastructure
{
    public interface IDbHelper<T> where T : class, IDto
    {
        Task<T> Find(string id, CancellationToken cancellationToken = default);
        Task<T> Find(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        Task Add(T mutatedDocument, CancellationToken cancellationToken = default);

        Task Set<TField>(string id, Expression<Func<T, TField>> field, TField value,
            CancellationToken cancellationToken = default);

        Task Set<TField>(Expression<Func<T, bool>> filter, Expression<Func<T, TField>> field, TField value,
            CancellationToken cancellationToken = default);

        Task Mutate(T projection, CancellationToken cancellationToken = default);
        Task Mutate(string id, Action<T> mutator, CancellationToken cancellationToken = default);
        Task Mutate(Expression<Func<T, bool>> filter, Action<T> mutator, CancellationToken cancellationToken = default);

        Task Delete(string id, CancellationToken cancellationToken = default);
        Task Delete(T projection, CancellationToken cancellationToken = default);
        Task Delete(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
    }
}