using System;

namespace Erazer.Framework.Events
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventAttribute: Attribute
    {
        public string Name { get; }

        public EventAttribute(string name)
        {
            Name = name;
        }
    }
}
