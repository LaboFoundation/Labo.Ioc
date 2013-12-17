namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Unity;

    using NUnit.Framework;

    [TestFixture]
    public class UnityContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new UnityIocContainer();
        }
    }
}