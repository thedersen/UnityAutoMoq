using System;

namespace UnityAutoMoq.Tests
{
    public class LazyService
    {
        public Func<IService> ServiceFunc;

        public LazyService(Func<IService> serviceFunc)
        {
            ServiceFunc = serviceFunc;
        }
    }
}