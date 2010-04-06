using Moq;

namespace UnityAutoMoq
{
    public interface IMoqContainer
    {
        Mock<T> GetMock<T>() where T : class;
    }
}