using System;
using System.Linq.Expressions;

namespace Sammlung.CommandLine.Reflection
{
    /// <summary>
    /// The <see cref="PropertyFactory"/> creates <see cref="Property{T1,T2}"/> types.
    /// </summary>
    public static class PropertyFactory
    {
        private static Action<T1, T2> CreateSetValue<T1, T2>(Expression<Func<T1, T2>> getValue)
        {
            var memberExpr = (MemberExpression)getValue.Body;
            var @this = Expression.Parameter(typeof(T1), "$this");
            var value = Expression.Parameter(typeof(T2), "value");

            var assignExpression = Expression.Assign(Expression.MakeMemberAccess(@this, memberExpr.Member), value);
            var lambdaExpression = Expression.Lambda<Action<T1, T2>>(assignExpression, @this, value);
            return lambdaExpression.Compile();
        }

        /// <summary>
        /// Creates the <see cref="Property{T1,T2}"/> object using only the getter expression.
        /// </summary>
        /// <param name="expression">the getter expression</param>
        /// <typeparam name="T1">the object type</typeparam>
        /// <typeparam name="T2">the type of the property on the object type</typeparam>
        /// <returns>the created property</returns>
        public static Property<T1, T2> Property<T1, T2>(Expression<Func<T1, T2>> expression)
        {
            var getValue = expression.Compile();
            var setValue = CreateSetValue(expression);
            return new Property<T1, T2>(getValue, setValue);
        }
    }
}