using Erazer.Framework.Domain;

namespace Erazer.DAL.Events
{
    public interface ISubscription<T> where T : AggregateRoot
    {
        void Connect();
    }
}
