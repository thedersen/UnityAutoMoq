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
        private readonly Dictionary<Type, object> mocks;
        private readonly DefaultValue defaultValue;

        public UnityAutoMoqBuilderStrategy(IEnumerable<Type> registeredTypes, DefaultValue defaultValue)
        {
            this.registeredTypes = registeredTypes;
            this.defaultValue = defaultValue;
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

            genericType.InvokeMember("DefaultValue", BindingFlags.SetProperty, null, mock, new object[] { defaultValue });

            object mockedInstance = genericType.InvokeMember("Object", BindingFlags.GetProperty, null, mock, null);
            mocks.Add(t, mockedInstance);

            return mockedInstance;
        }
    }
}