using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Moq;

namespace UnityAutoMoq
{
    public class UnityAutoMoqContainer : UnityContainer
    {
        private readonly Dictionary<Type, AsExpression> asExpressions;

        public UnityAutoMoqContainer()
            : this(DefaultValue.Mock)
        {
        }

        public UnityAutoMoqContainer(DefaultValue defaultValue)
        {
            DefaultValue = defaultValue;
            AddExtension(new UnityAutoMoqExtension(this));

            asExpressions = new Dictionary<Type, AsExpression>();
        }

        public DefaultValue DefaultValue { get; set; }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T), null);
        }

        public Mock<T> GetMock<T>() where T : class
        {
            return Mock.Get(Resolve<T>());
        }

        public AsExpression ConfigureMock<T>()
        {
            var asExpression = new AsExpression(typeof(T));
            asExpressions.Add(typeof(T),  asExpression);
            return asExpression;
        }

        internal AsExpression GetInterfaceImplementations(Type t)
        {
            return asExpressions.ContainsKey(t) ? asExpressions[t] : null;
        }
    }
}