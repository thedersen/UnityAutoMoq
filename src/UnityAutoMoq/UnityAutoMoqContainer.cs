using System;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Moq;

namespace UnityAutoMoq
{
    public class UnityAutoMoqContainer : UnityContainer, IMoqContainer
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

        public AsExpression As<T>()
        {
            return new AsExpression(typeof(T), this);
        }

        public Mock<T> GetMock<T>() where T : class
        {
            return GetMock<T>(null);
        }

        public Mock<T> GetMock<T>(params Type[] implements) where T : class
        {
            var instance = (T)GetOrCreateMockedInstance(typeof (T), implements);

            return Mock.Get(instance);
        }

        private object GetOrCreateMockedInstance(Type t)
        {
            return GetOrCreateMockedInstance(t, null);
        }

        private object GetOrCreateMockedInstance(Type t, params Type[] implements)
        {
            var instance = TryResolve(t, null);
            if (instance == null)
            {
                instance = CreateMockedInstance(t, implements);
                RegisterInstance(t, instance);
            }
            return instance;
        }

        private object CreateMockedInstance(Type t, params Type[] implements)
        {
            Type genericType = typeof (Mock<>).MakeGenericType(new[] {t});

            object instance = Activator.CreateInstance(genericType);

            if(implements != null)
                implements.Each(type => genericType.GetMethod("As").MakeGenericMethod(type).Invoke(instance, null));

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