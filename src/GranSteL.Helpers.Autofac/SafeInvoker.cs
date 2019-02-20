using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;

namespace GranSteL.Helpers.Autofac
{
    public class SafeInvoker<T> : ISafeInvoker<T>
    {
        private readonly Func<Owned<T>> _buildDependency;

        public SafeInvoker(Func<Owned<T>> buildDependency)
        {
            _buildDependency = buildDependency;
        }

        public TOut Invoke<TOut>(Func<T, TOut> action)
        {
            using (var dependency = _buildDependency())
            {
                return action(dependency.Value);
            }
        }

        public async Task<TOut> InvokeAsync<TOut>(Func<T, Task<TOut>> action)
        {
            using (var dependency = _buildDependency())
            {
                return await action(dependency.Value);
            }
        }

        public void Invoke(Action<T> action)
        {
            using (var dependency = _buildDependency())
            {
                action(dependency.Value);
            }
        }

        public async Task InvokeAsync(Func<T, Task> action)
        {
            using (var dependency = _buildDependency())
            {
                await action(dependency.Value);
            }
        }
    }
}
