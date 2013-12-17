namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Dynamo;

    using NUnit.Framework;

    [TestFixture]
    public class DynamoContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new DynamoIocContainer();
        }
    }
}