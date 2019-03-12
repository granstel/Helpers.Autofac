namespace GranSteL.Helpers.Autofac.Tests
{
    public interface IFixture
    {
        void ReturnVoid();

        T ReturnValue<T>();
    }
}
