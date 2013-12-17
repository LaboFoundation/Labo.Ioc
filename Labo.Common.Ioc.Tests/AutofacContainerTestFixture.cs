namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Autofac;

    using NUnit.Framework;

    [TestFixture]
    public class AutofacContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new AutofacIocContainer();
        }
    }
}