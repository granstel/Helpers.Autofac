using System.Threading.Tasks;
using NUnit.Framework;

namespace GranSteL.Helpers.Autofac.Tests
{
    [TestFixture]
    public partial class SafeInvokerTests
    {
        [Test]
        public async Task Asynchronous_AsyncNested_Success()
        {
            _testFixture.Setup(f => f.Test());

            await _invoker.InvokeAsync(async d =>
            {
                await _invoker.InvokeAsync(t => t.TestAsync());

                Assert.DoesNotThrowAsync(async () => await d.TestAsync());
            });

            _testFixture.VerifyAll();
        }

        [Test]
        public async Task Asynchronous_SyncNested_Success()
        {
            _testFixture.Setup(f => f.Test());

            await _invoker.InvokeAsync(async d =>
            {
                await _invoker.Invoke(t => t.TestAsync());

                Assert.DoesNotThrowAsync(async () => await d.TestAsync());
            });

            _testFixture.VerifyAll();
        }

        [Test]
        public void Synchronous_AsyncNested_Success()
        {
            _testFixture.Setup(f => f.Test());

            _invoker.Invoke(d =>
            {
                _invoker.InvokeAsync(t => t.TestAsync());

                Assert.DoesNotThrow(d.Test);
            });

            _testFixture.VerifyAll();
        }

        [Test]
        public void Synchronous_SyncNestedSuccess()
        {
            _testFixture.Setup(f => f.Test());

            _invoker.Invoke(d =>
            {
                _invoker.Invoke(t => t.Test());

                Assert.DoesNotThrow(d.Test);
            });

            _testFixture.VerifyAll();
        }
    }
}
