using System;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
// ReSharper disable InvalidXmlDocComment

namespace GranSteL.Helpers.Autofac
{
    /// <summary>
    /// Represents a dependency that can be released by the dependent component.
    /// Depend on System.Func&lt;Owned&lt;T&gt;&gt; in order to create and dispose of other components as required.
    /// </summary>
    /// <typeparam name="T">The service provided by the dependency.</typeparam>
    /// <remarks>
    /// <para>
    /// Autofac automatically provides instances of <see cref="Owned{T}"/> whenever the
    /// service <typeparamref name="T"/> is registered.
    /// </para>
    /// <para>
    /// It is not necessary for <typeparamref name="T"/>, or the underlying component, to implement <see cref="System.IDisposable"/>.
    /// Disposing of the <see cref="Owned{T}"/> object is the correct way to handle cleanup of the dependency,
    /// as this will dispose of any other components created indirectly as well.
    /// </para>
    /// <para>
    /// When <see cref="Owned{T}"/> is resolved, a new <see cref="ILifetimeScope"/> is created for the
    /// underlying <typeparamref name="T"/>, and tagged with the service matching <typeparamref name="T"/>,
    /// generally a <see cref="TypedService"/>. This means that shared instances can be tied to this
    /// scope by registering them as InstancePerMatchingLifetimeScope(new TypedService(typeof(T))).
    /// </para>
    /// </remarks>
    public class SafeInvoker<T> : ISafeInvoker<T>
    {
        private readonly Func<Owned<T>> _buildDependency;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeInvoker{T}"/> class.
        /// </summary>
        /// <param name="buildDependency">
        /// The value representing the instances of <see cref="Owned{T}"/>,
        /// wrapped by System.Func&lt;Owned&lt;T&gt;&gt; in order to create and dispose of other components as required.</param>
        public SafeInvoker(Func<Owned<T>> buildDependency)
        {
            _buildDependency = buildDependency;
        }

        /// <summary>Invokes a encapsulated by System.Func&lt;&gt; method that returns a value of the type specified by the
        /// <paramref name="TResult">TResult</paramref> parameter.</summary>
        /// <param name="action">The encapsulated method.</param>
        /// <typeparam name="T">The type of the parameter of the encapsulated method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the encapsulated method.</typeparam>
        /// <returns>An instance of <paramref name="TResult">TResult</paramref></returns>
        public TResult Invoke<TResult>(Func<T, TResult> action)
        {
            using (var dependency = _buildDependency())
            {
                return action(dependency.Value);
            }
        }

        /// <summary>Invokes a encapsulated by System.Func&lt;&gt; asynchronous method that returns a value of the type specified by the
        /// <paramref name="TResult">TResult</paramref> parameter.</summary>
        /// <param name="action">The encapsulated asynchronous method.</param>
        /// <typeparam name="T">The type of the parameter of the encapsulated asynchronous method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the encapsulated asynchronous method.</typeparam>
        /// <returns>An instance of <paramref name="TResult">TResult</paramref></returns>
        public async Task<TResult> InvokeAsync<TResult>(Func<T, Task<TResult>> action)
        {
            using (var dependency = _buildDependency())
            {
                return await action(dependency.Value);
            }
        }

        /// <summary>Invokes a encapsulated by System.Action&lt;&gt; method that
        /// has a single parameter and does not return a value.</summary>
        /// <param name="action">The encapsulated method.</param>
        /// <typeparam name="T">The type of the parameter of the encapsulated method.</typeparam>
        public void Invoke(Action<T> action)
        {
            using (var dependency = _buildDependency())
            {
                action(dependency.Value);
            }
        }

        /// <summary>Invokes a encapsulated by System.Action&lt;&gt; asynchronous method that
        /// has a single parameter and does not return a value.</summary>
        /// <param name="action">The encapsulated asynchronous method.</param>
        /// <typeparam name="T">The type of the parameter of the encapsulated asynchronous method.</typeparam>
        public async Task InvokeAsync(Func<T, Task> action)
        {
            using (var dependency = _buildDependency())
            {
                await action(dependency.Value);
            }
        }
    }
}
