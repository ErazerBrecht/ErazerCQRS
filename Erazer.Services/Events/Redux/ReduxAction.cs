namespace Erazer.Services.Events.Redux
{
    public class ReduxAction
    {
        public string Type { get; private set; }
        public object Payload { get; private set; }

        // TODO Change object to generic of type ViewModel
        public ReduxAction(string type, object payload)
        {
            Type = type;
            Payload = payload;
        }

    }
}