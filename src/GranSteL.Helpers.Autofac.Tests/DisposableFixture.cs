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

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
