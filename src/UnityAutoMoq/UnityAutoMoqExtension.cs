using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Moq;

namespace UnityAutoMoq
{
    public class UnityAutoMoqExtension : UnityContainerExtension
    {
        private readonly IList<Type> registeredTypes = new List<Type>();
        private readonly DefaultValue defaultValue;

        public UnityAutoMoqExtension(DefaultValue defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        protected override void Initialize()
        {
            KeepTrackOfRegisteredInterfaces();
            GenerateMocksForUnregisteredInterfaces();
        }

        private void KeepTrackOfRegisteredInterfaces()
        {
            Context.Registering += (sender, e) => RegisterInterface(e.TypeFrom ?? e.TypeTo);
            Context.RegisteringInstance += (sender, e) => RegisterInterface(e.RegisteredType);
        }

        private void RegisterInterface(Type typeToRegister)
        {
            if (typeToRegister.IsInterface) 
                registeredTypes.Add(typeToRegister);
        }

        private void GenerateMocksForUnregisteredInterfaces()
        {
            Context.Strategies.Add(new UnityAutoMoqBuilderStrategy(registeredTypes, defaultValue), UnityBuildStage.PreCreation);
        }
    }
}