namespace Erazer.Services.Events.Mappings
{
    public class ReduxAction
    {
        public string Type { get; private set; }
        public object Payload { get; private set; }
    }
}