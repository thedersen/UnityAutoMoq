using System;

namespace UnityAutoMoq.Test
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