using NUnit.Framework;
using Moq;
using NUnit.Framework.SyntaxHelpers;

namespace UnityAutoMoq.Test
{
    [TestFixture]
    public class UnityAutoMoqContainerFixture
    {
        private UnityAutoMoqContainer _container;

        [SetUp]
        public void SetUp()
        {
            _container = new UnityAutoMoqContainer();
        }

        [Test]
        public void Can_get_instance_without_registering_it_first()
        {
            var mocked = _container.Resolve<IService>();

            Assert.That(mocked, Is.Not.Null);
        }

        [Test]
        public void Can_get_mock()
        {
            Mock<IService> mock = _container.GetMock<IService>();

            Assert.That(mock, Is.Not.Null);
        }

        [Test]
        public void Mocked_object_and_resolved_instance_should_be_the_same()
        {
            Mock<IService> mock = _container.GetMock<IService>();
            var mocked = _container.Resolve<IService>();
            
            Assert.That(mock.Object, Is.SameAs(mocked));
        }

        [Test]
        public void Mocked_object_and_resolved_instance_should_be_the_same_order_independent()
        {
            var mocked = _container.Resolve<IService>();
            Mock<IService> mock = _container.GetMock<IService>();

            Assert.That(mock.Object, Is.SameAs(mocked));
        }

        [Test]
        public void Should_apply_default_value_when_creating_mocks()
        {
            _container.DefaultValue = DefaultValue.Mock;
            var mocked = _container.GetMock<IService>();

            Assert.That(mocked.DefaultValue, Is.EqualTo(DefaultValue.Mock));
        }

        [Test]
        public void Can_resolve_concrete_type_with_dependency()
        {
            var concrete = _container.Resolve<Service>();

            Assert.That(concrete, Is.Not.Null);
            Assert.That(concrete.AnotherService, Is.Not.Null);
        }

        [Test]
        public void Getting_mock_after_resolving_concrete_type_should_return_the_same_mock_as_passed_as_argument_to_the_concrete()
        {
            var concrete = _container.Resolve<Service>();
            Mock<IAnotherService> mock = _container.GetMock<IAnotherService>();

            Assert.That(concrete.AnotherService, Is.SameAs(mock.Object));
        }
    }
}