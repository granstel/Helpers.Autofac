using AutoFixture;
using System.Threading.Tasks;
using Moq;
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

                Assert.DoesNotThrowAsync(d.TestAsync);
            });

            _testFixture.Verify(f => f.ReturnVoid(), Times.Exactly(2));
        }

        [Test]
        public async Task Asynchronous_SyncNested_Success()
        {
            _testFixture.Setup(f => f.ReturnVoid());

            await _invoker.InvokeAsync(async d =>
            {
                await _invoker.Invoke(t => t.TestAsync());

                Assert.DoesNotThrowAsync(d.TestAsync);
            });

            _testFixture.Verify(f => f.ReturnVoid(), Times.Exactly(2));
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

            _testFixture.Verify(f => f.ReturnValue<int>(), Times.Exactly(2));

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

            _testFixture.Verify(f => f.ReturnValue<int>(), Times.Exactly(2));

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

            _testFixture.Verify(f => f.ReturnVoid(), Times.Exactly(2));
        }

        [Test]
        public void Synchronous_SyncNested_Success()
        {
            _testFixture.Setup(f => f.ReturnVoid());

            _invoker.Invoke(d =>
            {
                _invoker.Invoke(t => t.Test());

                Assert.DoesNotThrow(d.Test);
            });

            _testFixture.Verify(f => f.ReturnVoid(), Times.Exactly(2));
        }

        [Test]
        public void Synchronous_SyncReturnsValue_Success()
        {
            var expected = _fixture.Create<int>();

            _testFixture.Setup(f => f.ReturnValue<int>()).Returns(expected);

            var result = _invoker.Invoke(d =>
            {
                var firstResult = _invoker.Invoke(t => t.TestValue<int>());

                var secondResult = d.TestValue<int>();

                return firstResult + secondResult;
            });

            _testFixture.Verify(f => f.ReturnValue<int>(), Times.Exactly(2));

            Assert.AreEqual(expected * 2, result);
        }

        #endregion Synchronous
    }
}
