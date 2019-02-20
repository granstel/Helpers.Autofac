using System;
using System.Threading.Tasks;

namespace GranSteL.Helpers.Autofac
{
    public interface ISafeInvoker<out T>
    {
        TOut Invoke<TOut>(Func<T, TOut> action);

        Task<TOut> InvokeAsync<TOut>(Func<T, Task<TOut>> action);

        void Invoke(Action<T> action);

        Task InvokeAsync(Func<T, Task> action);
    }
}