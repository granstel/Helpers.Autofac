using System;
using System.Threading.Tasks;
using Autofac;
using AutoFixture;
using Moq;
using NUnit.Framework;

namespace GranSteL.Helpers.Autofac.Tests
{
    [TestFixture]
    public partial class SafeInvokerTests
    {
        private MockRepository _mockRepository;

        private ISafeInvoker<DisposableFixture> _invoker;

        private Mock<IFixture> _testFixture;

        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _testFixture = _mockRepository.Create<IFixture>();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterGeneric(typeof(SafeInvoker<>)).As(typeof(ISafeInvoker<>)).SingleInstance();
            containerBuilder.RegisterType<DisposableFixture>().InstancePerDependency();
            containerBuilder.Register(c => _testFixture.Object).As<IFixture>();

            var container = containerBuilder.Build();

            _invoker = container.Resolve<ISafeInvoker<DisposableFixture>>();

            _fixture = new Fixture { OmitAutoProperties = true };
        }

        [Test]
        public async Task Synchronous_AsyncNested_ReturnsVoid_Throws()
        {
            _testFixture.Setup(f => f.ReturnVoid());

            await _invoker.Invoke(async d =>
            {
                await _invoker.InvokeAsync(t => t.TestAsync());

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsync());
            });

            _testFixture.VerifyAll();
        }

        [Test]
        public async Task Synchronous_SyncNested_ReturnsVoid_Throws()
        {
            _testFixture.Setup(f => f.ReturnVoid());

            await _invoker.Invoke(async d =>
            {
                await _invoker.Invoke(t => t.TestAsync());

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsync());
            });

            _testFixture.VerifyAll();
        }
    }
}