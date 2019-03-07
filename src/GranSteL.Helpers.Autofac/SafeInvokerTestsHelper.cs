using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac.Features.OwnedInstances;

namespace GranSteL.Helpers.Autofac
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
    }
}
