using System;
using System.Collections.Generic;
using Moq;

namespace UnityAutoMoq
{
    public class AsExpression : IMoqContainer
    {
        private readonly List<Type> implements = new List<Type>();
        private readonly UnityAutoMoqContainer unityAutoMoqContainer;

        public AsExpression(Type implements, UnityAutoMoqContainer unityAutoMoqContainer)
        {
            this.implements.Add(implements);
            this.unityAutoMoqContainer = unityAutoMoqContainer;
        }

        public AsExpression And<T>() where T : class
        {
            implements.Add(typeof(T));
            return this;
        } 

        public Mock<T> GetMock<T>() where T : class
        {
            return unityAutoMoqContainer.GetMock<T>(implements.ToArray());
        }
    }
}