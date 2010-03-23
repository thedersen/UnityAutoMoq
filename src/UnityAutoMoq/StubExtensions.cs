using System;
using System.Linq.Expressions;
using System.Reflection;
using Moq;

namespace UnityAutoMoq
{
    public static class StubExtensions
    {
        public static void Stub<T, TProperty>(this Mock<T> mock, Expression<Func<T, TProperty>> propertyExpression) where T : class
        {
            mock.Stub(propertyExpression, default(TProperty));
        }

        public static void Stub<T, TProperty>(this Mock<T> mock, Expression<Func<T, TProperty>> propertyExpression, TProperty initialValue)  where T : class
        {
            TProperty[] value = { initialValue };
            mock.SetupGet(propertyExpression).Returns(() => value[0]);

            var memberExpression = (MemberExpression)propertyExpression.Body;
            var propertyInfo = (PropertyInfo)memberExpression.Member;

            if (propertyInfo.CanWrite)
            {
                mock.SetupSet(propertyExpression).Callback(p => value[0] = p);
            }
        }
    }
}