using System;
using System.Threading.Tasks;
using Autofac;
using AutoFixture;
using GranSteL.Helpers.Autofac.Tests.Fixtures;
using Moq;
using NUnit.Framework;

namespace GranSteL.Helpers.Autofac.Tests
{
    [TestFixture]
    public partial class SafeInvokerTests
    {
        private MockRepository _mockRepository;

        private ISafeInvoker<DisposableFixture> _target;

        private Mock<Fixtures.IFixture> _testFixture;

        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _testFixture = _mockRepository.Create<Fixtures.IFixture>();

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterGeneric(typeof(SafeInvoker<>)).As(typeof(ISafeInvoker<>)).SingleInstance();
            containerBuilder.RegisterType<DisposableFixture>().InstancePerDependency();
            containerBuilder.Register(c => _testFixture.Object).As<Fixtures.IFixture>();

            var container = containerBuilder.Build();

            _target = container.Resolve<ISafeInvoker<DisposableFixture>>();

            _fixture = new Fixture { OmitAutoProperties = true };
        }

        [Test]
        public async Task Synchronous_AsyncNested_ReturnsVoid_Throws()
        {
            _testFixture.Setup(f => f.ReturnVoid());

            await _target.Invoke(async d =>
            {
                await _target.InvokeAsync(t => t.TestAsync());

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsync());
            });

            _testFixture.VerifyAll();
        }

        [Test]
        public async Task Synchronous_SyncNested_ReturnsVoid_Throws()
        {
            _testFixture.Setup(f => f.ReturnVoid());

            await _target.Invoke(async d =>
            {
                await _target.Invoke(t => t.TestAsync());

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsync());
            });

            _testFixture.VerifyAll();
        }

        [Test]
        public void Synchronous_AsyncReturnsValue_Success()
        {
            var expected = _fixture.Create<int>();

            _testFixture.Setup(f => f.ReturnValue<int>()).Returns(expected);

            var result = _target.Invoke(async d =>
            {
                var firstResult = await _target.InvokeAsync(t => t.TestAsyncValue<int>());

                var secondResult = await d.TestAsyncValue<int>();

                return firstResult + secondResult;
            });
            
            //Excepted, that type of "result" variable is integer, but it isn't
            Assert.AreNotEqual(typeof(int), result.GetType());
        }

        [Test]
        public async Task Synchronous_AsyncNested_ReturnsValue_Throws()
        {
            var expected = _fixture.Create<int>();

            _testFixture.Setup(f => f.ReturnValue<int>()).Returns(expected);

            await _target.Invoke(async d =>
            {
                await _target.InvokeAsync(t => t.TestAsyncValue<int>());

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsyncValue<int>());
            });

            _testFixture.VerifyAll();
        }
    }
}