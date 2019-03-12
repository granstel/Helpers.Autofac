using AutoFixture;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GranSteL.Helpers.Autofac.Tests
{
    [TestFixture]
    public partial class SafeInvokerTests
    {
        #region Asynchronous

        [Test]
        public async Task Asynchronous_AsyncNested_Success()
        {
            _testFixture.Setup(f => f.ReturnVoid());

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
            _testFixture.Setup(f => f.ReturnVoid());

            await _invoker.InvokeAsync(async d =>
            {
                await _invoker.Invoke(t => t.TestAsync());

                Assert.DoesNotThrowAsync(async () => await d.TestAsync());
            });

            _testFixture.VerifyAll();
        }

        [Test]
        public async Task Asynchronous_AsyncReturnsValue_Success()
        {
            var expected = _fixture.Create<int>();

            _testFixture.Setup(f => f.ReturnValue<int>()).Returns(expected);

            var result = await _invoker.InvokeAsync(async d =>
            {
                var firstResult = await _invoker.InvokeAsync(t => t.TestAsyncValue<int>());

                var secondResult = await d.TestAsyncValue<int>();

                return firstResult + secondResult;
            });

            _testFixture.VerifyAll();

            Assert.AreEqual(expected * 2, result);
        }

        [Test]
        public async Task Asynchronous_SyncReturnsValue_Success()
        {
            var expected = _fixture.Create<int>();

            _testFixture.Setup(f => f.ReturnValue<int>()).Returns(expected);

            var result = await _invoker.InvokeAsync(async d =>
            {
                var firstResult = await _invoker.InvokeAsync(t => t.TestAsyncValue<int>());

                var secondResult = await d.TestAsyncValue<int>();

                return firstResult + secondResult;
            });

            _testFixture.VerifyAll();

            Assert.AreEqual(expected * 2, result);
        }

        #endregion Asynchronous

        #region Synchronous

        [Test]
        public void Synchronous_AsyncNested_Success()
        {
            _testFixture.Setup(f => f.ReturnVoid());

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
            _testFixture.Setup(f => f.ReturnVoid());

            _invoker.Invoke(d =>
            {
                _invoker.Invoke(t => t.Test());

                Assert.DoesNotThrow(d.Test);
            });

            _testFixture.VerifyAll();
        }

        #endregion Synchronous
    }
}
