using Microsoft.Practices.Unity;
using Moq;

namespace UnityAutoMoq
{
    public class UnityAutoMoqContainer : UnityContainer, IMoqContainer
    {
        public UnityAutoMoqContainer()
            : this(DefaultValue.Mock)
        {
        }

        public UnityAutoMoqContainer(DefaultValue defaultValue)
        {
            AddExtension(new UnityAutoMoqExtension(defaultValue));
        }

        public Mock<T> GetMock<T>() where T : class
        {
            return Mock.Get(this.Resolve<T>());
        }
    }
}