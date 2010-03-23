using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace UnityAutoMoq.Test.Test
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void Test()
        {
            var unityContainer = new UnityContainer();

            unityContainer
                .RegisterType<INterFace, NterFace>("1")
                .RegisterType<INterFace, NterFace2>("2")
                .RegisterType<IInterface, Interface>();

            var obj = unityContainer.Resolve<IInterface>();
        }
    }
}