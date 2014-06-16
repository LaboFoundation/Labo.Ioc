namespace Labo.Common.Ioc.Tests.Performance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    using Labo.Common.Diagnostics;
    using Labo.Common.Ioc.Container;
    using Labo.Common.Ioc.Tests.Performance.Domain;

    public class B
    {
        private readonly A m_A = (A)C.Func();
    }

    public class A
    {
        public A()
        {
            
        }
    }

    internal static class C
    {
        public static Func<object> Func;
    }

    class Program
    {
        static void Main(string[] args)
        {
            //IocContainer container = new IocContainer();
            //container.RegisterSingleInstance<ILogger, Logger>();
            //container.RegisterSingleInstance<IConfigurationManager, ConfigurationManager>();
            //container.RegisterSingleInstance<ISettings, Settings>();
            //container.RegisterSingleInstance<IErrorHandler, ErrorHandler>();
            //container.RegisterSingleInstance<IController, Controller>();
            //container.RegisterSingleInstance<IApplication, Application>();

            //container.GetInstance<IApplication>();

            //ExecutionWatch executionWatch = new ExecutionWatch(Timing.StopwatchFactory());

            //const int times = 10 * 1000 * 1000;

            //Type type = typeof(IApplication);
            //IntPtr intPtr = type.TypeHandle.Value;
            //long @long = intPtr.ToInt64();
            //int @int = intPtr.ToInt32();

            //Console.WriteLine("Type.GetHashCode() : {0}", executionWatch.Measure(ExecutionWatchOnStart, () => type.GetHashCode(), null, times));
            //Console.WriteLine("IntPtr.GetHashCode() : {0}", executionWatch.Measure(ExecutionWatchOnStart, () => intPtr.GetHashCode(), null, times));
            //Console.WriteLine("long.GetHashCode() : {0}", executionWatch.Measure(ExecutionWatchOnStart, () => @long.GetHashCode(), null, times));
            //Console.WriteLine("int.GetHashCode() : {0}", executionWatch.Measure(ExecutionWatchOnStart, () => @int.GetHashCode(), null, times));

            //Type[] types = typeof(Type).Assembly.GetTypes().Take(20).ToArray();
            //Dictionary<long, string> longDictionary = types.Select(x => new { Key = x.TypeHandle.Value.ToInt64(), x.FullName }).ToDictionary(x => x.Key, x => x.FullName);
            //Dictionary<Type, string> typeDictionary = types.Select(x => new { Key = x, x.FullName }).ToDictionary(x => x.Key, x => x.FullName);
            //Dictionary<IntPtr, string> intPtrDictionary = types.Select(x => new { Key = x.TypeHandle.Value, x.FullName }).ToDictionary(x => x.Key, x => x.FullName);

            //Console.WriteLine("Type Dictionary : {0}", executionWatch.Measure(ExecutionWatchOnStart, () => typeDictionary[types[5]].ToString(), null, times));
            //Console.WriteLine("IntPtr Dictionary : {0}", executionWatch.Measure(ExecutionWatchOnStart, () => intPtrDictionary[types[5].TypeHandle.Value].ToString(), null, times));
            //Console.WriteLine("long Dictionary : {0}", executionWatch.Measure(ExecutionWatchOnStart, () => longDictionary[types[5].TypeHandle.Value.ToInt64()].ToString(), null, times));

            //Console.ReadKey();
        }

        //private static void ExecutionWatchOnStart()
        //{
        //    GC.Collect();
        //}
    }
}
