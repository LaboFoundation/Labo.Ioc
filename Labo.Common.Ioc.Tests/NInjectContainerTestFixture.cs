namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.NInject;

    using NUnit.Framework;

    [TestFixture]
    public class NInjectContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new NInjectIocContainer();
        }
    }
}