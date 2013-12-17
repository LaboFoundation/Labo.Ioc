namespace Labo.Common.Ioc.Tests
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    public interface ITestService
    {
        Guid Guid { get; set; }
    }

    public class TestServiceImplementation : ITestService
    {
        public Guid Guid { get; set; }

        public TestServiceImplementation()
        {
            Guid = Guid.NewGuid();
        }
    }

    public interface ISingletonService
    {
        Guid Guid { get; }
    }

    public interface ITransientService
    {
        Guid Guid { get; }
    }

    public interface ISingletonTransientService : ISingletonService
    {
        ITransientService TransientService { get; }
    }

    public interface ITransientSingletonService : ITransientService
    {
        ISingletonService SingletonService { get; }
    }

    public sealed class SingletonService : ISingletonService
    {
        public Guid Guid { get; set; }

        public SingletonService()
        {
            Guid = Guid.NewGuid();
        }
    }

    public sealed class SingletonTransientService : ISingletonTransientService
    {
        private readonly ITransientService m_TransientService;
        public ITransientService TransientService
        {
            get { return m_TransientService; }
        }

        public Guid Guid { get; set; }

        public SingletonTransientService(ITransientService transientService)
        {
            m_TransientService = transientService;

            Guid = Guid.NewGuid();
        }
    }

    public sealed class TransientSingletonService : ITransientSingletonService
    {
        private readonly ISingletonService m_SingletonService;
        public ISingletonService SingletonService
        {
            get { return m_SingletonService; }
        }

        public Guid Guid { get; set; }

        public TransientSingletonService(ISingletonService singletonService)
        {
            m_SingletonService = singletonService;

            Guid = Guid.NewGuid();
        }
    }

    public sealed class TransientService : ITransientService
    {
        public Guid Guid { get; set; }

        public TransientService()
        {
            Guid = Guid.NewGuid();
        }
    }

    [TestFixture]
    public abstract class IocContainerTestFixture
    {
        public abstract IIocContainer CreateContainer();

        [Test]
        public void RegisterSingletonTransientService()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstance(typeof(ITransientService), typeof(TransientService));
            iocContainer.RegisterSingleInstance(typeof(ISingletonTransientService), typeof(SingletonTransientService));

            AssertRegisterSingletonTransientService(iocContainer);
        }

        [Test]
        public void RegisterSingletonTransientServiceGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstance<ITransientService, TransientService>();
            iocContainer.RegisterSingleInstance<ISingletonTransientService, SingletonTransientService>();

            AssertRegisterSingletonTransientService(iocContainer);
        }

        [Test]
        public void RegisterSingletonTransientServiceLambda()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstance<ITransientService>(x => new TransientService());
            iocContainer.RegisterSingleInstance<ISingletonTransientService>(x => new SingletonTransientService(x.GetInstance<ITransientService>()));

            AssertRegisterSingletonTransientService(iocContainer);
        }

        private static void AssertRegisterSingletonTransientService(IIocContainerResolver iocContainer)
        {
            Assert.AreSame(
                iocContainer.GetInstance<ISingletonTransientService>(),
                iocContainer.GetInstance<ISingletonTransientService>());
            Assert.AreEqual(
                iocContainer.GetInstance<ISingletonTransientService>().Guid,
                iocContainer.GetInstance<ISingletonTransientService>().Guid);
            Assert.AreSame(
                iocContainer.GetInstance<ISingletonTransientService>().TransientService,
                iocContainer.GetInstance<ISingletonTransientService>().TransientService);
            Assert.AreEqual(
                iocContainer.GetInstance<ISingletonTransientService>().TransientService.Guid,
                iocContainer.GetInstance<ISingletonTransientService>().TransientService.Guid);
        }

        [Test]
        public void RegisterTransientSingletonService()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance(typeof(ISingletonService), typeof(SingletonService));
            iocContainer.RegisterInstance(typeof(ITransientSingletonService), typeof(TransientSingletonService));

            AssertRegisterTransientSingletonService(iocContainer);
        }

        [Test]
        public void RegisterTransientSingletonServiceGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance<ISingletonService, SingletonService>();
            iocContainer.RegisterInstance<ITransientSingletonService, TransientSingletonService>();

            AssertRegisterTransientSingletonService(iocContainer);
        }

        [Test]
        public void RegisterTransientSingletonServiceLambda()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance<ISingletonService>(x => new SingletonService());
            iocContainer.RegisterInstance<ITransientSingletonService>(x => new TransientSingletonService(x.GetInstance<ISingletonService>()));

            AssertRegisterTransientSingletonService(iocContainer);
        }

        private static void AssertRegisterTransientSingletonService(IIocContainerResolver iocContainer)
        {
            Assert.AreNotSame(
                iocContainer.GetInstance<ITransientSingletonService>(),
                iocContainer.GetInstance<ITransientSingletonService>());
            Assert.AreNotEqual(
                iocContainer.GetInstance<ITransientSingletonService>().Guid,
                iocContainer.GetInstance<ITransientSingletonService>().Guid);
            Assert.AreSame(
                iocContainer.GetInstance<ITransientSingletonService>().SingletonService,
                iocContainer.GetInstance<ITransientSingletonService>().SingletonService);
            Assert.AreEqual(
                iocContainer.GetInstance<ITransientSingletonService>().SingletonService.Guid,
                iocContainer.GetInstance<ITransientSingletonService>().SingletonService.Guid);
        }

        [Test]
        public void RegisterSingleInstanceLambda()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance<ITestService>(x => new TestServiceImplementation());

            AssertRegisterSingleInstance<ITestService>(iocContainer);
        }

        [Test]
        public void RegisterSingleInstanceGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance<ITestService, TestServiceImplementation>();

            AssertRegisterSingleInstance<ITestService>(iocContainer);
        }

        [Test]
        public void RegisterSingleInstance()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstance(typeof(TestServiceImplementation));

            AssertRegisterSingleInstance<TestServiceImplementation>(iocContainer);
        }

        private static void AssertRegisterSingleInstance<TService>(IIocContainerResolver iocContainer)
           where TService : class, ITestService
        {
            ITestService testService = iocContainer.GetInstance<TService>();
            Assert.IsNotNull(testService);

            ITestService secondServiceInstance = iocContainer.GetInstance<TService>();
            Assert.AreSame(testService, secondServiceInstance);
            Assert.AreEqual(testService.Guid, secondServiceInstance.Guid);
        }

        [Test]
        public void RegisterSingleInstanceNamed()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstanceNamed(typeof(TestServiceImplementation), "TestService1");
            iocContainer.RegisterSingleInstanceNamed(typeof(TestServiceImplementation), "TestService2");

            AssertRegisterSingleInstanceNamed<TestServiceImplementation>(iocContainer);
        }

        [Test]
        public void RegisterSingleInstanceNamedLambda()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstanceNamed<ITestService>(x => new TestServiceImplementation(), "TestService1");
            iocContainer.RegisterSingleInstanceNamed<ITestService>(x => new TestServiceImplementation(), "TestService2");

            AssertRegisterSingleInstanceNamed<ITestService>(iocContainer);
        }

        [Test]
        public void RegisterSingleInstanceNamedGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterSingleInstanceNamed<ITestService, TestServiceImplementation>("TestService1");
            iocContainer.RegisterSingleInstanceNamed<ITestService, TestServiceImplementation>("TestService2");

            AssertRegisterSingleInstanceNamed<ITestService>(iocContainer);
        }

        private static void AssertRegisterSingleInstanceNamed<TService>(IIocContainerResolver iocContainer)
            where TService : ITestService
        {
            TService[] allInstances = iocContainer.GetAllInstances<TService>().ToArray();
            Assert.IsNotNull(allInstances);
            Assert.AreEqual(2, allInstances.Length);

            TService testService1 = iocContainer.GetInstance<TService>("TestService1");
            Assert.IsNotNull(testService1);

            TService secondTestService1 = iocContainer.GetInstance<TService>("TestService1");
            Assert.AreSame(testService1, secondTestService1);
            Assert.AreEqual(testService1.Guid, secondTestService1.Guid);

            TService testService2 = iocContainer.GetInstance<TService>("TestService2");
            Assert.AreNotSame(testService1, testService2);
            Assert.AreNotEqual(testService1.Guid, testService2.Guid);

            Assert.IsTrue(allInstances.Contains(testService1));
            Assert.IsTrue(allInstances.Contains(testService2));
        }

        [Test]
        public void RegisterInstanceLambda()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstance<ITestService>(x => new TestServiceImplementation());

            AssertRegisterInstance<ITestService>(iocContainer);
        }

        [Test]
        public void RegisterInstance()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstance(typeof(TestServiceImplementation));

            AssertRegisterInstance<TestServiceImplementation>(iocContainer);
        }

        [Test]
        public void RegisterInstanceGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstance<ITestService, TestServiceImplementation>();

            AssertRegisterInstance<ITestService>(iocContainer);
        }

        private static void AssertRegisterInstance<TService>(IIocContainerResolver iocContainer)
            where TService : class, ITestService
        {
            ITestService testService = iocContainer.GetInstance<TService>();
            Assert.IsNotNull(testService);

            ITestService secondServiceInstance = iocContainer.GetInstance<TService>();
            Assert.AreNotSame(testService, secondServiceInstance);
            Assert.AreNotEqual(testService.Guid, secondServiceInstance.Guid);
        }

        [Test]
        public void RegisterInstanceNamed()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstanceNamed(typeof(TestServiceImplementation), "TestService1");
            iocContainer.RegisterInstanceNamed(typeof(TestServiceImplementation), "TestService2");

            AssertRegisterInstanceNamed<TestServiceImplementation>(iocContainer);
        }

        [Test]
        public void RegisterInstanceNamedLambda()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstanceNamed<ITestService>(x => new TestServiceImplementation(), "TestService1");
            iocContainer.RegisterInstanceNamed<ITestService>(x => new TestServiceImplementation(), "TestService2");

            AssertRegisterInstanceNamed<ITestService>(iocContainer);
        }

        [Test]
        public void RegisterInstanceNamedGeneric()
        {
            IIocContainer iocContainer = CreateContainer();
            iocContainer.RegisterInstanceNamed<ITestService, TestServiceImplementation>("TestService1");
            iocContainer.RegisterInstanceNamed<ITestService, TestServiceImplementation>("TestService2");

            AssertRegisterInstanceNamed<ITestService>(iocContainer);
        }

        private static void AssertRegisterInstanceNamed<TService>(IIocContainerResolver iocContainer)
            where TService : ITestService
        {
            TService[] allInstances = iocContainer.GetAllInstances<TService>().ToArray();
            Assert.IsNotNull(allInstances);
            Assert.AreEqual(2, allInstances.Length);

            TService testService1 = iocContainer.GetInstance<TService>("TestService1");
            Assert.IsNotNull(testService1);

            TService secondTestService1 = iocContainer.GetInstance<TService>("TestService1");
            Assert.AreNotSame(testService1, secondTestService1);
            Assert.AreNotEqual(testService1.Guid, secondTestService1.Guid);

            TService testService2 = iocContainer.GetInstance<TService>("TestService2");
            Assert.AreNotSame(testService1, testService2);
            Assert.AreNotEqual(testService1.Guid, testService2.Guid);

            Assert.IsFalse(allInstances.Contains(testService1));
            Assert.IsFalse(allInstances.Contains(testService2));
        }

        [Test]
        public void GetInstanceOptional()
        {
            IIocContainer iocContainer = CreateContainer();
            Assert.IsNull(iocContainer.GetInstanceOptional(typeof(ITestService)));
            Assert.IsNull(iocContainer.GetInstanceOptional<ITestService>());
            Assert.IsNull(iocContainer.GetInstanceOptionalByName(typeof(ITestService), "TestService"));
            Assert.IsNull(iocContainer.GetInstanceOptionalByName<ITestService>("TestService"));

            // The container can't be changed after the first call to GetInstance, GetAllInstances and Verify (Simple injector).
            // So we recreate container
            iocContainer = CreateContainer();

            iocContainer.RegisterInstance<ITestService, TestServiceImplementation>();
            Assert.IsNotNull(iocContainer.GetInstanceOptional(typeof(ITestService)));
            Assert.IsNotNull(iocContainer.GetInstanceOptional<ITestService>());

            // The container can't be changed after the first call to GetInstance, GetAllInstances and Verify (Simple injector).
            // So we recreate container
            iocContainer = CreateContainer();

            iocContainer.RegisterInstanceNamed<ITestService, TestServiceImplementation>("TestService");
            Assert.IsNotNull(iocContainer.GetInstanceOptionalByName(typeof(ITestService), "TestService"));
            Assert.IsNotNull(iocContainer.GetInstanceOptionalByName<ITestService>("TestService"));
        }

        [Test]
        public void IsRegisteredNoRegistration()
        {
            IIocContainer iocContainer = CreateContainer();

            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation), "TestService"));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(ITestService)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(ITestService), "TestService"));
            Assert.IsFalse(iocContainer.IsRegistered<ITestService>());
            Assert.IsFalse(iocContainer.IsRegistered<ITestService>("TestService"));
        }

        [Test]
        public void IsRegisteredGeneric()
        {
            IIocContainer iocContainer = CreateContainer();

            iocContainer.RegisterInstance<ITestService, TestServiceImplementation>();
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation), "TestService"));
            Assert.IsTrue(iocContainer.IsRegistered(typeof(ITestService)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(ITestService), "TestService"));
            Assert.IsTrue(iocContainer.IsRegistered<ITestService>());
            Assert.IsFalse(iocContainer.IsRegistered<ITestService>("TestService"));
        }

        [Test]
        public void IsRegisteredNonGeneric()
        {
            IIocContainer iocContainer = CreateContainer();

            iocContainer.RegisterInstance(typeof(ITestService), typeof(TestServiceImplementation));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation), "TestService"));
            Assert.IsTrue(iocContainer.IsRegistered(typeof(ITestService)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(ITestService), "TestService"));
            Assert.IsTrue(iocContainer.IsRegistered<ITestService>());
            Assert.IsFalse(iocContainer.IsRegistered<ITestService>("TestService"));
        }

        [Test]
        public void IsRegisteredWithNameGeneric()
        {
            IIocContainer iocContainer = CreateContainer();

            iocContainer.RegisterInstanceNamed<ITestService, TestServiceImplementation>("TestService");
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation), "TestService"));
            Assert.IsTrue(iocContainer.IsRegistered(typeof(ITestService)));
            Assert.IsTrue(iocContainer.IsRegistered(typeof(ITestService), "TestService"));
            Assert.IsTrue(iocContainer.IsRegistered<ITestService>());
            Assert.IsTrue(iocContainer.IsRegistered<ITestService>("TestService"));
        }

        [Test]
        public void IsRegisteredWithNameNonGenerice()
        {
            IIocContainer iocContainer = CreateContainer();

            iocContainer.RegisterInstanceNamed(typeof(ITestService), typeof(TestServiceImplementation), "TestService");
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation), "TestService"));
            Assert.IsTrue(iocContainer.IsRegistered(typeof(ITestService)));
            Assert.IsTrue(iocContainer.IsRegistered(typeof(ITestService), "TestService"));
            Assert.IsTrue(iocContainer.IsRegistered<ITestService>());
            Assert.IsTrue(iocContainer.IsRegistered<ITestService>("TestService"));
        }

        [Test]
        public void IsRegisteredWithImplementationClass()
        {
            IIocContainer iocContainer = CreateContainer();

            iocContainer.RegisterInstance(typeof(TestServiceImplementation));
            Assert.IsTrue(iocContainer.IsRegistered(typeof(TestServiceImplementation)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(TestServiceImplementation), "TestService"));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(ITestService)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(ITestService), "TestService"));
            Assert.IsFalse(iocContainer.IsRegistered<ITestService>());
            Assert.IsFalse(iocContainer.IsRegistered<ITestService>("TestService"));
        }

        [Test]
        public void IsRegisteredNamedWithImplementationClass()
        {
            IIocContainer iocContainer = CreateContainer();

            iocContainer.RegisterInstanceNamed(typeof(TestServiceImplementation), "TestService");
            Assert.IsTrue(iocContainer.IsRegistered(typeof(TestServiceImplementation)));
            Assert.IsTrue(iocContainer.IsRegistered(typeof(TestServiceImplementation), "TestService"));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(ITestService)));
            Assert.IsFalse(iocContainer.IsRegistered(typeof(ITestService), "TestService"));
            Assert.IsFalse(iocContainer.IsRegistered<ITestService>());
            Assert.IsFalse(iocContainer.IsRegistered<ITestService>("TestService"));
        }
    }
}
