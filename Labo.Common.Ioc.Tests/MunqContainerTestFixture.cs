namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Munq;

    using NUnit.Framework;

    [TestFixture]
    public class MunqContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new MunqIocContainer();
        }
    }
}