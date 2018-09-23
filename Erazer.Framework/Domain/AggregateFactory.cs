using System;
using System.Linq.Expressions;
using Erazer.Framework.Exceptions;

namespace Erazer.Framework.Domain
{
    internal static class AggregateFactory<T> where T : AggregateRoot
    {
        private static readonly Func<T> _constructor = CreateTypeConstructor();

        private static Func<T> CreateTypeConstructor()
        {
            try
            {
                var newExpr = Expression.New(typeof(T));
                var func = Expression.Lambda<Func<T>>(newExpr);
                return func.Compile();
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public static T Build()
        {
            if (_constructor == null)
                throw new MissingParameterLessConstructorException(typeof(T));
            return _constructor();
        }
    }
}
