namespace Labo.Common.Ioc.Tests
{
    using Labo.Common.Ioc.Container.Exceptions;

    using NUnit.Framework;

    [TestFixture]
    public class LaboContainerTestFixture : IocContainerTestFixture
    {
        public override IIocContainer CreateContainer()
        {
            return new Container.IocContainer();
        }

        private interface ICircularDependencyClass1
        {
        }

        private interface ICircularDependencyClass2
        {
        }

        private interface ICircularDependencyClass3
        {  
        }

        private class CircularDependencyClass1 : ICircularDependencyClass1
        {
            private readonly ICircularDependencyClass2 m_Dependency;

            public CircularDependencyClass1(ICircularDependencyClass2 dependency)
            {
                m_Dependency = dependency;
            }
        }

        private class CircularDependencyClass2 : ICircularDependencyClass2
        {
            private readonly ICircularDependencyClass3 m_Dependency;

            public CircularDependencyClass2(ICircularDependencyClass3 dependency)
            {
                m_Dependency = dependency;
            }
        }

        private class CircularDependencyClass3 : ICircularDependencyClass3
        {
            private readonly ICircularDependencyClass2 m_Dependency;

            public CircularDependencyClass3(ICircularDependencyClass2 dependency)
            {
                m_Dependency = dependency;
            }
        }

        [Test]
        public void ResolveDetectCircularDependency()
        {
            Container.IocContainer container = new Container.IocContainer();
            container.RegisterSingleInstance<ICircularDependencyClass1, CircularDependencyClass1>();
            container.RegisterSingleInstance<ICircularDependencyClass2, CircularDependencyClass2>();
            container.RegisterSingleInstance<ICircularDependencyClass3, CircularDependencyClass3>();

            Assert.Throws<IocContainerDependencyResolutionException>(() => container.GetInstance<ICircularDependencyClass1>());

            container = new Container.IocContainer();
            container.RegisterInstance<ICircularDependencyClass1, CircularDependencyClass1>();
            container.RegisterInstance<ICircularDependencyClass2, CircularDependencyClass2>();
            container.RegisterInstance<ICircularDependencyClass3, CircularDependencyClass3>();

            Assert.Throws<IocContainerDependencyResolutionException>(() => container.GetInstance<ICircularDependencyClass1>());
        }

        // TODO: Write tests related to labo ioc container
    }
}