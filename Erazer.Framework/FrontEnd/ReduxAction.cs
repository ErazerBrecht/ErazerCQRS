namespace Erazer.Framework.FrontEnd
{
    public abstract class ReduxAction<T> where T : IViewModel
    {
        public string Type { get; private set; }
        public T Payload { get; private set; }

        public ReduxAction(string type, T payload)
        {
            Type = type;
            Payload = payload;
        }
    }
}