using System;

namespace Erazer.Framework.Exceptions
{
    public class MissingParameterLessConstructorException : Exception
    {
        public MissingParameterLessConstructorException(Type type) : base(
            $"{type.FullName} has no constructor without parameters. This can be either public or private")
        {

        }
    }
}
