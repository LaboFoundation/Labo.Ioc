namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Linfu;

    using NUnit.Framework;

    [TestFixture]
    public class LinfuContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new LinfuIocContainer();
        }
    }
}