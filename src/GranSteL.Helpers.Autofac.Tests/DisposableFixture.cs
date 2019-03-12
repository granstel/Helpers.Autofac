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

        internal async Task<T> TestAsync<T>(Func<T> action)
        {
            CheckDisposed();

            return await Task.Run(() => action.Invoke());
        }

        internal async Task TestAsync()
        {
            CheckDisposed();

            await Task.Run(() => _fixture.Test());
        }

        internal T Test<T>(Func<T> action)
        {
            CheckDisposed();

            return action.Invoke();
        }

        internal void Test()
        {
            CheckDisposed();

            _fixture.Test();
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
