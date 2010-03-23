using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Moq;

namespace UnityAutoMoq
{
    public class UnityAutoMoqContainer : UnityContainer
    {
        public UnityAutoMoqContainer()
        {
            DefaultValue = DefaultValue.Mock;
        }

        public DefaultValue DefaultValue { get; set; }

        public override object Resolve(Type t, string name)
        {
            if (!t.IsInterface)
                return ResolveConcreteType(t, name);


            return GetOrCreateMockedInstance(t);
        }

        public Mock<T> GetMock<T>() where T : class
        {
            var instanse = (T)GetOrCreateMockedInstance(typeof (T));

            return Mock.Get(instanse);
        }

        private object GetOrCreateMockedInstance(Type t)
        {
            var instance = TryResolve(t, null);
            if (instance == null)
            {
                instance = CreateMockedInstance(t);
                RegisterInstance(t, instance);
            }
            return instance;
        }

        private object CreateMockedInstance(Type t)
        {
            Type genericType = typeof (Mock<>).MakeGenericType(new[] {t});

            object instance = Activator.CreateInstance(genericType);

            genericType.InvokeMember("DefaultValue", BindingFlags.SetProperty, null, instance, new object[] {DefaultValue});
            
            return genericType.InvokeMember("Object", BindingFlags.GetProperty, null, instance, null);
        }

        private object ResolveConcreteType(Type t, string name)
        {
            CreateMocksForAllDependenciesForConcreteType(t);

            return base.Resolve(t, name);
        }

        private void CreateMocksForAllDependenciesForConcreteType(Type t)
        {
            ConstructorInfo[] constructors = t.GetConstructors();
            foreach (var constructor in constructors)
            {
                ParameterInfo[] parameters = constructor.GetParameters();

                foreach (var parameter in parameters.Where(p => p.ParameterType.IsInterface))
                {
                    GetOrCreateMockedInstance(parameter.ParameterType);
                }
            }
        }

        private object TryResolve(Type t, string name)
        {
            object resolvedObject = null;
            try
            {
                resolvedObject = base.Resolve(t, name);
            }
            catch (ResolutionFailedException)
            {
            }
            
            return resolvedObject;
        }
    }
}