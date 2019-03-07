using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using GranSteL.Helpers.Autofac;
using GranSteL.Helpers.Autofac.Tests;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private ISafeInvoker<DisposableFixture> _invoker;

        [SetUp]
        public void Setup()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterGeneric(typeof(SafeInvoker<>)).As(typeof(ISafeInvoker<>)).SingleInstance();
            containerBuilder.RegisterType<DisposableFixture>();

            var container = containerBuilder.Build();

            _invoker = container.Resolve<ISafeInvoker<DisposableFixture>>();
        }

        [Test]
        public async Task NestedInvokes_InvalidInvoke_Throws()
        {
            await _invoker.InvokeAsync(async d =>
            {
                var result1 =  _invoker.Invoke(t => t.TestAsync(() =>
                {
                     Thread.Sleep(1000);
                     return 5;
                }));

                var result2 = await d.TestAsync(async () =>
                {
                    return (await (result1)) + 1;
                });

                //Assert.AreEqual(result2, ++result1);
            });

            Thread.Sleep(3000);
        }
    }
}