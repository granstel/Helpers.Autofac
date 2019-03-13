using AutoFixture;
using GranSteL.Helpers.Autofac.Tests.Fixtures;
using Moq;
using NUnit.Framework;

namespace GranSteL.Helpers.Autofac.Tests
{
    public class SafeInvokerTestsHelperExample
    {
        private MockRepository _mockRepository;

        private Mock<Fixtures.IFixture> _testFixture;

        private DependencyFixture _target;

        private Fixture _autoFixture;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new MockRepository(MockBehavior.Strict);

            _testFixture = _mockRepository.Create<Fixtures.IFixture>();

            var safeInvoker = SafeInvokerTestsHelper.CreateSafeInvoker(_testFixture.Object);

            _target = new DependencyFixture(safeInvoker);

            _autoFixture = new Fixture { OmitAutoProperties = true };
        }

        [Test]
        public void Test_ReturnValue_Success()
        {
            var expected = _autoFixture.Create<int>();

            _testFixture.Setup(f => f.ReturnValue<int>()).Returns(expected);


            var result = _target.Test();


            _mockRepository.VerifyAll();

            Assert.AreEqual(expected, result);
        }
    }
}
