namespace GranSteL.Helpers.Autofac.Tests.Fixtures
{
    internal class DependencyFixture
    {
        private readonly ISafeInvoker<IFixture> _fixture;

        public DependencyFixture(ISafeInvoker<IFixture> fixture)
        {
            _fixture = fixture;
        }

        internal int Test()
        {
            var result = _fixture.Invoke(f => f.ReturnValue<int>());

            return result;
        }
    }
}
