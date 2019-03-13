namespace GranSteL.Helpers.Autofac.Tests.Fixtures
{
    public interface IFixture
    {
        void ReturnVoid();

        T ReturnValue<T>();
    }
}
