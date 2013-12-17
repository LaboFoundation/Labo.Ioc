namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.StructureMap;

    using NUnit.Framework;

    [TestFixture]
    public class StructureMapContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new StructureMapIocContainer();
        }
    }
}