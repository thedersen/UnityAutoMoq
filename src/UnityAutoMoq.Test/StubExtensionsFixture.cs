using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace UnityAutoMoq.Test
{
    [TestFixture]
    public class StubExtensionsFixture
    {
        [Test]
        public void Can_stub_property()
        {
            var mock = new Mock<IService>();
            mock.Stub(x => x.Property);
            mock.Object.Property = "SomeValue";

            Assert.That(mock.Object.Property, Is.EqualTo("SomeValue"));
        }

        [Test]
        public void Can_stub_with_default_value()
        {
            var mock = new Mock<IService>();
            mock.Stub(x => x.Property, "SomeValue");

            Assert.That(mock.Object.Property, Is.EqualTo("SomeValue"));
        }

        [Test]
        public void Can_stub_property_without_setter()
        {
            var mock = new Mock<IService>();

            mock.Stub(x => x.PropertyWithoutSetter, "SomeValue");

            Assert.That(mock.Object.PropertyWithoutSetter, Is.EqualTo("SomeValue"));
        }
    }
}