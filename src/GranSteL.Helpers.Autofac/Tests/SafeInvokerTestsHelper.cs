using System;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac.Features.OwnedInstances;

namespace GranSteL.Helpers.Autofac.Tests
{
    public static class SafeInvokerTestsHelper
    {
        public static ISafeInvoker<T> CreateSafeInvoker<T>(T value) where T : class
        {
            var lifeTime = new LifetimeScope(new ComponentRegistry());

            var owned = new Owned<T>(value, lifeTime);

            var safeInvoker = new SafeInvoker<T>(() => owned);

            return safeInvoker;
        }

        public static ISafeInvoker<T> CreateSafeInvoker<T>(Func<T> getValue) where T : class
        {
            var lifeTime = new LifetimeScope(new ComponentRegistry());

            var safeInvoker = new SafeInvoker<T>(() => new Owned<T>(getValue(), lifeTime));

            return safeInvoker;
        }
    }
}
