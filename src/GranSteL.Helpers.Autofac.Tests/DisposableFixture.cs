using System;
using System.Threading.Tasks;

namespace GranSteL.Helpers.Autofac.Tests
{
    internal class DisposableFixture : IDisposable
    {
        private bool _disposed;

        internal async Task<T> TestAsync<T>(Func<T> action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DisposableFixture), $"{nameof(DisposableFixture)} already disposed");
            }

            return await Task.Run(() => action.Invoke());
        }

        internal async Task TestAsync(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DisposableFixture), $"{nameof(DisposableFixture)} already disposed");
            }

            await Task.Run(() => action.Invoke());
        }

        internal T Test<T>(Func<T> action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DisposableFixture), $"{nameof(DisposableFixture)} already disposed");
            }

            return action.Invoke();
        }

        internal void Test(Action action)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(DisposableFixture), $"{nameof(DisposableFixture)} already disposed");
            }

            action.Invoke();
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
