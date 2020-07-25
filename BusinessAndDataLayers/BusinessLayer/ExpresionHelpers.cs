using System;
using System.Linq.Expressions;

namespace BusinessLayerLib
{
    public static class ExpresionHelpers
    {
        public static Expression CreateQueryExpression<T>(Expression<Func<T, bool>> predicate)
        {
            return predicate;
        }
    }
}
