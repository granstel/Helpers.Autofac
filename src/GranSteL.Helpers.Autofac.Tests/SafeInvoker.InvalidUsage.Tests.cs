using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoFixture;
using NUnit.Framework;

namespace GranSteL.Helpers.Autofac.Tests
{
    [TestFixture]
    public partial class SafeInvokerTests
    {
        private ISafeInvoker<DisposableFixture> _invoker;

        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterGeneric(typeof(SafeInvoker<>)).As(typeof(ISafeInvoker<>)).SingleInstance();
            containerBuilder.RegisterType<DisposableFixture>();

            var container = containerBuilder.Build();

            _invoker = container.Resolve<ISafeInvoker<DisposableFixture>>();

            _fixture = new Fixture { OmitAutoProperties = true };
        }

        [Test]
        public async Task SynchronousMainInvoke_AsyncNestedReturnsVoids_Throws()
        {
            await _invoker.Invoke(async d =>
            {
                await _invoker.InvokeAsync(t => t.TestAsync(() =>
                {
                     Thread.Sleep(1000);
                }));

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsync(() =>
                {
                    Thread.Sleep(1000);
                }));
            });

            Thread.Sleep(3000);
        }

        [Test]
        public async Task SynchronousMainInvoke_SyncNestedReturnsVoids_Throws()
        {
            await _invoker.Invoke(async d =>
            {
                await _invoker.Invoke(t => t.TestAsync(() =>
                {
                    Thread.Sleep(1000);
                }));

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsync(() =>
                {
                    Thread.Sleep(1000);
                }));
            });

            Thread.Sleep(3000);
        }

        [Test]
        public async Task SynchronousMainInvoke_SyncNestedReturns_Throws()
        {
            var result = _fixture.Create<int>();

            await _invoker.Invoke(async d =>
            {
                var firstReturns = await _invoker.Invoke(t => t.TestAsync(() =>
                {
                    Thread.Sleep(1000);

                    return result;
                }));

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsync(() =>
                {
                    Thread.Sleep(1000);

                    return firstReturns + 1;
                }));
            });

            Thread.Sleep(3000);
        }

        [Test]
        public async Task SynchronousMainInvoke_AsyncNestedReturns_Throws()
        {
            var result = _fixture.Create<int>();

            await _invoker.Invoke(async d =>
            {
                var firstReturns = await _invoker.InvokeAsync(t => t.TestAsync(() =>
                {
                    Thread.Sleep(1000);

                    return result;
                }));

                Assert.ThrowsAsync<ObjectDisposedException>(async () => await d.TestAsync(() =>
                {
                    Thread.Sleep(1000);

                    return firstReturns + 1;
                }));
            });

            Thread.Sleep(3000);
        }
    }
}