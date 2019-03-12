using System;
using System.Threading.Tasks;

namespace GranSteL.Helpers.Autofac.Tests
{
    internal class DisposableFixture : IDisposable
    {
        private readonly IFixture _fixture;

        private bool _disposed;

        public DisposableFixture(IFixture fixture)
        {
            _fixture = fixture;
        }

        internal async Task<T> TestAsyncValue<T>()
        {
            CheckDisposed();

            return await Task.Run(() => _fixture.ReturnValue<T>());
        }

        internal async Task TestAsync()
        {
            CheckDisposed();

            await Task.Run(() => _fixture.ReturnVoid());
        }

        internal T TestValue<T>()
        {
            CheckDisposed();

            return _fixture.ReturnValue<T>();
        }

        internal void Test()
        {
            CheckDisposed();

            _fixture.ReturnVoid();
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DisposableFixture), $"{nameof(DisposableFixture)} already disposed");
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
