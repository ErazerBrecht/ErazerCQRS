namespace Erazer.Framework.FrontEnd
{
    public abstract class ReduxAction<T> where T : IViewModel
    {
        public string Type { get; }
        public T Payload { get; }

        public ReduxAction(string type, T payload)
        {
            Type = type;
            Payload = payload;
        }
    }
}