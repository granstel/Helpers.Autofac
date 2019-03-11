using AutoFixture;
using System.Threading;
using NUnit.Framework;

namespace GranSteL.Helpers.Autofac.Tests
{
    [TestFixture]
    public partial class SafeInvokerTests
    {
        [Test]
        public void Synchronous_SyncNested_ReturnsValue_Success()
        {
            var result = _fixture.Create<int>();

            _invoker.Invoke(d =>
            {
                var firstReturns = _invoker.Invoke(t => t.Test(() =>
                {
                    Thread.Sleep(1000);

                    return result;
                }));

                Assert.DoesNotThrow(() => d.Test(() =>
                {
                    Thread.Sleep(1000);

                    return firstReturns + 1;
                }));
            });
        }

        [Test]
        public void Synchronous_AsyncNested_ReturnsValue_Success()
        {
            var result = _fixture.Create<int>();

            _invoker.Invoke(async d =>
            {
                var firstReturns = await _invoker.Invoke(t => t.TestAsync(() =>
                {
                    Thread.Sleep(1000);

                    return result;
                }));

                Assert.DoesNotThrow(() => d.Test(() =>
                {
                    Thread.Sleep(1000);

                    return firstReturns + 1;
                }));
            });
        }
    }
}
