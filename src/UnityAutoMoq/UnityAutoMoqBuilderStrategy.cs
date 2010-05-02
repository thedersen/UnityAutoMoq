using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Moq;

namespace UnityAutoMoq
{
    public class UnityAutoMoqBuilderStrategy : BuilderStrategy
    {
        private readonly IEnumerable<Type> registeredTypes;
        private readonly UnityAutoMoqContainer autoMoqContainer;
        private readonly Dictionary<Type, object> mocks;

        public UnityAutoMoqBuilderStrategy(IEnumerable<Type> registeredTypes, UnityAutoMoqContainer autoMoqContainer)
        {
            this.registeredTypes = registeredTypes;
            this.autoMoqContainer = autoMoqContainer;
            mocks = new Dictionary<Type, object>();
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            var type = context.OriginalBuildKey.Type;

            if (type.IsInterface && TypeIsNotRegistered(type))
            {
                context.Existing = GetOrCreateMock(type);
            }
        }

        private bool TypeIsNotRegistered(Type type)
        {
            return !registeredTypes.Any(x => x.Equals(type));
        }

        private object GetOrCreateMock(Type t)
        {
            if (mocks.ContainsKey(t))
                return mocks[t];

            Type genericType = typeof(Mock<>).MakeGenericType(new[] { t });

            object mock = Activator.CreateInstance(genericType);

            AsExpression interfaceImplementations = autoMoqContainer.GetInterfaceImplementations(t);
            if(interfaceImplementations != null)
                interfaceImplementations.GetImplementations().Each(type => genericType.GetMethod("As").MakeGenericMethod(type).Invoke(mock, null));

            genericType.InvokeMember("DefaultValue", BindingFlags.SetProperty, null, mock, new object[] { autoMoqContainer.DefaultValue });

            object mockedInstance = genericType.InvokeMember("Object", BindingFlags.GetProperty, null, mock, null);
            mocks.Add(t, mockedInstance);

            return mockedInstance;
        }
    }
}