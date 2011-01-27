using System;
using NUnit.Framework;
using Moq;

namespace UnityAutoMoq.Tests
{
    [TestFixture]
    public class UnityAutoMoqContainerFixture
    {
        private UnityAutoMoqContainer container;

        [SetUp]
        public void SetUp()
        {
            container = new UnityAutoMoqContainer();
        }

        [Test]
        public void Can_get_instance_without_registering_it_first()
        {
            var mocked = container.Resolve<IService>();

            mocked.ShouldNotBeNull();
        }

        [Test]
        public void Can_get_mock()
        {
            Mock<IService> mock = container.GetMock<IService>();

            mock.ShouldNotBeNull();
        }

        [Test]
        public void Mocked_object_and_resolved_instance_should_be_the_same()
        {
            Mock<IService> mock = container.GetMock<IService>();
            var mocked = container.Resolve<IService>();

            mock.Object.ShouldBeSameAs(mocked);
        }

        [Test]
        public void Mocked_object_and_resolved_instance_should_be_the_same_order_independent()
        {
            var mocked = container.Resolve<IService>();
            Mock<IService> mock = container.GetMock<IService>();

            mock.Object.ShouldBeSameAs(mocked);
        }

        [Test]
        public void Should_apply_default_default_value_when_none_specified()
        {
            container = new UnityAutoMoqContainer();
            var mocked = container.GetMock<IService>();

            mocked.DefaultValue.ShouldEqual(DefaultValue.Mock);
        }

        [Test]
        public void Should_apply_specified_default_value_when_specified()
        {
            container = new UnityAutoMoqContainer(DefaultValue.Empty);
            var mocked = container.GetMock<IService>();

            mocked.DefaultValue.ShouldEqual(DefaultValue.Empty);
        }

        [Test]
        public void Should_apply_specified_default_value_when_specified_2()
        {
            container = new UnityAutoMoqContainer{DefaultValue = DefaultValue.Empty};
            var mocked = container.GetMock<IService>();

            mocked.DefaultValue.ShouldEqual(DefaultValue.Empty);
        }

        [Test]
        public void Can_resolve_concrete_type_with_dependency()
        {
            var concrete = container.Resolve<Service>();

            concrete.ShouldNotBeNull();
            concrete.AnotherService.ShouldNotBeNull();
        }

        [Test]
        public void Getting_mock_after_resolving_concrete_type_should_return_the_same_mock_as_passed_as_argument_to_the_concrete()
        {
            var concrete = container.Resolve<Service>();
            Mock<IAnotherService> mock = container.GetMock<IAnotherService>();

            concrete.AnotherService.ShouldBeSameAs(mock.Object);
        }

        [Test]
        public void Can_configure_mock_as_several_interfaces()
        {
            container.ConfigureMock<IService>().As<IDisposable>();

            container.GetMock<IService>().As<IDisposable>();
        }

        [Test]
        public void Can_configure_mock_as_several_interfaces_2()
        {
            container.ConfigureMock<IService>().As<IDisposable>().As<IAnotherService>();

            container.GetMock<IService>().As<IDisposable>();
            container.GetMock<IService>().As<IAnotherService>();
        }

        [Test]
        public void Can_lazy_load_dependencies()
        {
            var service = container.Resolve<LazyService>();

            Assert.That(service.ServiceFunc(), Is.InstanceOf(typeof(IService)));
        }
    }
}