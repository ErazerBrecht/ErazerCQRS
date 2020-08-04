using System;
using Erazer.Framework.Factories;
using Erazer.Infrastructure.ReadStore;
using Erazer.Syncing.Infrastructure;
using Erazer.Syncing.Models;
using Erazer.Syncing.SeedWork;

namespace Erazer.Infrastructure.EventStore.Subscription
{
    public class SubscriptionDbUnitOfWorkFactory: IFactory<IDbUnitOfWork>
    {
        private readonly ISubscriptionContext _ctx;
        private readonly DbUnitOfWork _liveDb;
        private readonly DbBatchUnitOfWork _reSyncDb;

        public SubscriptionDbUnitOfWorkFactory(ISubscriptionContext ctx, DbUnitOfWork liveDb, DbBatchUnitOfWork reSyncDb)
        {
            _ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
            _liveDb = liveDb;
            _reSyncDb = reSyncDb;
        }
        
        public IDbUnitOfWork Build()
        {
            return _ctx.SubscriptionType switch
            {
                SubscriptionType.Live when _liveDb == null => throw new InvalidOperationException(
                    $"Unable to resolve {typeof(DbUnitOfWork).FullName} while trying to build an {typeof(IDbUnitOfWork).FullName} in {typeof(SubscriptionDbUnitOfWorkFactory).FullName}"),
                SubscriptionType.Live => _liveDb,
                SubscriptionType.ReSync when _reSyncDb == null => throw new InvalidOperationException(
                    $"Unable to resolve {typeof(DbBatchUnitOfWork).FullName} while trying to build an {typeof(IDbUnitOfWork).FullName} in {typeof(SubscriptionDbUnitOfWorkFactory).FullName}"),
                SubscriptionType.ReSync => _reSyncDb,
                _ => throw new NotImplementedException(
                    $"{typeof(SubscriptionDbUnitOfWorkFactory).FullName} doesn't know how to construct ${typeof(IDbUnitOfWork).FullName} when the subscription type is {_ctx.SubscriptionType}")
            };
        }
    }
}