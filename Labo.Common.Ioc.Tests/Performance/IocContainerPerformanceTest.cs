namespace Labo.Common.Ioc.Tests.Performance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    using Labo.Common.Ioc.DryIoc;
    using Labo.Common.Ioc.Dynamo;
    using Labo.Common.Ioc.HaveBox;
    using Labo.Common.Ioc.Hiro;
    using Labo.Common.Ioc.LightInject;
    using Labo.Common.Ioc.Munq;
    using Labo.Common.Ioc.SimpleInjector;
    using Labo.Common.Ioc.Tests.Performance.Domain;

    using NUnit.Framework;

    [TestFixture]
    public class IocContainerPerformanceTest
    {
        private sealed class NullContainer : IIocContainer
        {
            public bool IsRegistered<TService>(string name)
            {
                throw new NotImplementedException();
            }

            public bool IsRegistered<TService>()
            {
                throw new NotImplementedException();
            }

            public bool IsRegistered(Type type)
            {
                throw new NotImplementedException();
            }

            public bool IsRegistered(Type type, string name)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TService> GetAllInstances<TService>()
            {
                throw new NotImplementedException();
            }

            public IEnumerable<object> GetAllInstances(Type serviceType)
            {
                throw new NotImplementedException();
            }

            public TService GetInstance<TService>(object[] parameters) where TService : class
            {
                throw new NotImplementedException();
            }

            public TService GetInstance<TService>() where TService : class
            {
               return (TService)(object)new Application(new Controller(new ErrorHandler(new Logger(), new Settings(new ConfigurationManager()))));
            }

            public TService GetInstance<TService>(string name)
            {
                throw new NotImplementedException();
            }

            public object GetInstance(Type serviceType, object[] parameters)
            {
                throw new NotImplementedException();
            }

            public object GetInstance(Type serviceType)
            {
                throw new NotImplementedException();
            }

            public object GetInstanceByName(Type serviceType, string name, object[] parameters)
            {
                throw new NotImplementedException();
            }

            public object GetInstanceByName(Type serviceType, string name)
            {
                throw new NotImplementedException();
            }

            public object GetInstanceOptional(Type serviceType, object[] parameters)
            {
                throw new NotImplementedException();
            }

            public object GetInstanceOptional(Type serviceType)
            {
                throw new NotImplementedException();
            }

            public TService GetInstanceOptional<TService>(object[] parameters)
            {
                throw new NotImplementedException();
            }

            public TService GetInstanceOptional<TService>()
            {
                throw new NotImplementedException();
            }

            public TService GetInstanceOptionalByName<TService>(string name, object[] parameters)
            {
                throw new NotImplementedException();
            }

            public TService GetInstanceOptionalByName<TService>(string name)
            {
                throw new NotImplementedException();
            }

            public object GetInstanceOptionalByName(Type serviceType, string name, object[] parameters)
            {
                throw new NotImplementedException();
            }

            public object GetInstanceOptionalByName(Type serviceType, string name)
            {
                throw new NotImplementedException();
            }

            public void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator) where TImplementation : class
            {
            }

            public void RegisterSingleInstance<TService, TImplementation>() where TImplementation : TService
            {
            }

            public void RegisterSingleInstance(Type serviceType, Type implementationType)
            {
            }

            public void RegisterSingleInstance(Type serviceType)
            {
            }

            public void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name) where TImplementation : class
            {
            }

            public void RegisterSingleInstanceNamed<TService, TImplementation>(string name) where TImplementation : TService
            {
            }

            public void RegisterSingleInstanceNamed(Type serviceType, Type implementationType, string name)
            {
            }

            public void RegisterSingleInstanceNamed(Type serviceType, string name)
            {
            }

            public void RegisterInstance<TService, TImplementation>() where TImplementation : TService
            {
            }

            public void RegisterInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator) where TImplementation : class
            {
            }

            public void RegisterInstance(Type serviceType, Type implementationType)
            {
            }

            public void RegisterInstance(Type serviceType)
            {
            }

            public void RegisterInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name) where TImplementation : class
            {
            }

            public void RegisterInstanceNamed<TService, TImplementation>(string name) where TImplementation : TService
            {
            }

            public void RegisterInstanceNamed(Type serviceType, Type implementationType, string name)
            {
            }

            public void RegisterInstanceNamed(Type serviceType, string name)
            {
            }

            public void RegisterModule(IIocModule iocModule)
            {
            }
        }

        private static readonly HashSet<long> s_BatchIterations = new HashSet<long>() { 1000, 5000, 20000, 100000, 250000, 1000000, 2500000 };
        private static readonly Dictionary<string, Func<IIocContainer>> s_Containers = new Dictionary<string, Func<IIocContainer>>
                                                                                     {
                                                                                         { "Null", () => new NullContainer() },
                                                                                         //{ "NInject", () => new NInjectIocContainer() },
                                                                                         //{ "Linfu", () => new LinfuIocContainer() },
                                                                                         //{ "Unity", () => new UnityIocContainer() },
                                                                                         //{ "Autofac", () => new AutofacIocContainer() },
                                                                                         //{ "Mugen", () => new MugenIocContainer() },
                                                                                         //{ "TinyIoc", () => new TinyIocContainer() },
                                                                                         //{ "LightCore", () => new LightCoreIocContainer() },
                                                                                         { "Dynamo", () => new DynamoIocContainer() },
                                                                                         { "Hiro", () => new HiroIocContainer() },
                                                                                         { "Munq", () => new MunqIocContainer() },
                                                                                         { "LightInject", () => new LightInjectIocContainer() },
                                                                                         //{ "Structuremap", () => new StructureMapIocContainer() },
                                                                                         { "SimpleInjector", () => new SimpleInjectorIocContainer()},
                                                                                         { "Labo", () => new Container.IocContainer() },
                                                                                         { "Havebox", () => new HaveBoxIocContainer() },
                                                                                         { "Dry", () => new DryIocContainer() },
                                                                                     };
            
        [Test]
        public void TestPerformance()
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            TestPerformance(
                "Singleton;",
                container =>
                    {
                        container.RegisterSingleInstance<ILogger, Logger>();
                        container.RegisterSingleInstance<IConfigurationManager, ConfigurationManager>();
                        container.RegisterSingleInstance<ISettings, Settings>();
                        container.RegisterSingleInstance<IErrorHandler, ErrorHandler>();
                        container.RegisterSingleInstance<IController, Controller>();
                        container.RegisterSingleInstance<IApplication, Application>();
                    });

            TestPerformance(
               "Transient;",
               container =>
               {
                   container.RegisterInstance<ILogger, Logger>();
                   container.RegisterInstance<IConfigurationManager, ConfigurationManager>();
                   container.RegisterInstance<ISettings, Settings>();
                   container.RegisterInstance<IErrorHandler, ErrorHandler>();
                   container.RegisterInstance<IController, Controller>();
                   container.RegisterInstance<IApplication, Application>();
               });

            TestPerformance(
             "Combined;",
             container =>
             {
                 container.RegisterSingleInstance<ILogger, Logger>();
                 container.RegisterInstance<IConfigurationManager, ConfigurationManager>();
                 container.RegisterSingleInstance<ISettings, Settings>();
                 container.RegisterInstance<IErrorHandler, ErrorHandler>();
                 container.RegisterSingleInstance<IController, Controller>();
                 container.RegisterInstance<IApplication, Application>();
             });
        }

        private static void TestPerformance(string title, Action<IIocContainer> registerAction)
        {
            Console.WriteLine("\n{0}", title);

            Console.Write(string.Empty.PadRight(21, ' '));

            s_BatchIterations.ForEach(x => Console.Write(x.ToStringInvariant().PadRight(22, ' ')));
                
            List<PerformanceResult> performanceResults = new List<PerformanceResult>();
            List<string> containers = new List<string>();

            foreach (KeyValuePair<string, Func<IIocContainer>> containerEntry in s_Containers)
            {
                IIocContainer container = containerEntry.Value();

                registerAction(container);

                containers.Add(string.Format("\n{0}", containerEntry.Key).PadRight(22, ' '));

                s_BatchIterations.ForEach(
                    x =>
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();

                            //warm up
                            container.GetInstance<IApplication>();

                            PerformanceResult performanceResult = MeasurePerformance(() => container.GetInstance<IApplication>(), x);
                            performanceResult.IterationCount = x;
                            performanceResults.Add(performanceResult);
                        });
            }

            int batchIterationsCount = s_BatchIterations.Count;

            for (int i = 0; i < performanceResults.Count; i++)
            {
                if (i % batchIterationsCount == 0)
                {
                    Console.Write(i == 0 ? containers[i] : containers[i / batchIterationsCount]);
                }

                PerformanceResult performanceResult = performanceResults[i];
                long[] allPerformanceResults = performanceResults.Where(x => x.IterationCount == performanceResult.IterationCount).Select(x => x.Performance).ToArray();
                Console.Write(string.Format("{0} ({1})", performanceResult.Text, performanceResult.GetPerformanceOrder(allPerformanceResults)).PadRight(22, ' '));
            }
        }

        private struct PerformanceResult
        {
            public long IterationCount { get; set; }

            public string Text { get; set; }

            public long Performance { get; set; }

            public int GetPerformanceOrder(IList<long> allPerformanceResults)
            {
                allPerformanceResults = allPerformanceResults.OrderBy(x => x).ToList();

                for (int i = 0; i < allPerformanceResults.Count; i++)
                {
                    if (Performance == allPerformanceResults[i])
                    {
                        return i + 1;
                    }
                }

                return 0;
            }
        }

        private static PerformanceResult MeasurePerformance(Action action, decimal iterations)
        {
            GC.Collect();
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < iterations; i++)
            {
                action();
            }

            stopwatch.Stop();

            long performance = stopwatch.ElapsedTicks;

            return new PerformanceResult
                       {
                           Text = string.Format("{0} ({1})", performance.ToStringInvariant(), (performance / iterations).ToStringInvariant()),
                           Performance = performance
                       };
        }
    }
}
