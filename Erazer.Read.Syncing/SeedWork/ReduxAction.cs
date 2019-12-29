namespace Erazer.Syncing.SeedWork
{
    public abstract class ReduxAction<T> where T : class
    {
        public string Type { get; }
        public T Payload { get; }

        protected ReduxAction(string type, T payload)
        {
            Type = type;
            Payload = payload;
        }
    }
}