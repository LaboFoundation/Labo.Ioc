namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.LightInject;

    using NUnit.Framework;

    [TestFixture]
    public class LightInjectContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new LightInjectIocContainer();
        }
    }
}