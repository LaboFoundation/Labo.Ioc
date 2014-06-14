// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingletonServiceFactoryInvoker.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2013 Bora Akgun
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of
//   this software and associated documentation files (the "Software"), to deal in
//   the Software without restriction, including without limitation the rights to
//   use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//   the Software, and to permit persons to whom the Software is furnished to do so,
//   subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//   FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//   COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//   IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//   CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The singleton service factory invoker class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.CompilerServices;

    using Labo.Common.Reflection;
    using Labo.Common.Utils;

    /// <summary>
    /// The singleton service factory invoker class.
    /// </summary>
    internal sealed class SingletonServiceFactoryInvoker : IServiceFactoryInvoker
    {
        /// <summary>
        /// The constructor invoker cache
        /// </summary>
        private static readonly ConcurrentDictionary<ConstructorInfo, Lazy<object>> s_ConstructorInvokerCache = new ConcurrentDictionary<ConstructorInfo, Lazy<object>>();

        /// <summary>
        /// The service implementation type
        /// </summary>
        private readonly Type m_ServiceImplementationType;

        /// <summary>
        /// The service invoker function
        /// </summary>
        private readonly Func<object> m_ServiceInvokerFunc;

        /// <summary>
        /// The service invoker function lazy initializer
        /// </summary>
        private readonly Lazy<object> m_ServiceInvokerFuncLazyInitializer; 

        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonServiceFactoryInvoker"/> class.
        /// </summary>
        /// <param name="serviceInvokerFunc">The service invoker function.</param>
        public SingletonServiceFactoryInvoker(Func<object> serviceInvokerFunc)
        {
            m_ServiceInvokerFuncLazyInitializer = new Lazy<object>(serviceInvokerFunc, true);
            m_ServiceInvokerFunc = () => m_ServiceInvokerFuncLazyInitializer.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonServiceFactoryInvoker"/> class.
        /// </summary>
        /// <param name="serviceFactoryType">Type of the service factory.</param>
        /// <param name="serviceImplementationType">Type of the service implementation.</param>
        public SingletonServiceFactoryInvoker(Type serviceFactoryType, Type serviceImplementationType)
        {
            m_ServiceImplementationType = serviceImplementationType;

            DynamicMethod createServiceInstanceDynamicMethod = DynamicMethodHelper.CreateDynamicMethod("CreateServiceInstance", MethodAttributes.Static | MethodAttributes.Public, typeof(object), Type.EmptyTypes, serviceFactoryType);
            ILGenerator dynamicMethodGenerator = createServiceInstanceDynamicMethod.GetILGenerator();
            EmitHelper.Call(dynamicMethodGenerator, serviceFactoryType.GetMethod("CreateInstance"));
            EmitHelper.BoxIfValueType(dynamicMethodGenerator, typeof(object));
            EmitHelper.Ret(dynamicMethodGenerator);

            m_ServiceInvokerFunc = (Func<object>)createServiceInstanceDynamicMethod.CreateDelegate(typeof(Func<object>));
        }

        /// <summary>
        /// Gets service invoker function
        /// </summary>
        public Func<object> ServiceInvokerFunc
        {
            get
            {
                return m_ServiceInvokerFunc;
            }
        }

        /// <summary>
        /// Invokes the service factory.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The service instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object InvokeServiceFactory(object[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            Type[] parameterTypes = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                object parameter = parameters[i];
                parameterTypes[i] = TypeUtils.GetType(parameter);
            }

            ConstructorInfo constructor = m_ServiceImplementationType.GetConstructor(bindingFlags, null, parameterTypes, null);
            return s_ConstructorInvokerCache.GetOrAdd(constructor, c => new Lazy<object>(() => DynamicMethodHelper.EmitConstructorInvoker(m_ServiceImplementationType, c, parameterTypes)(parameters), true)).Value;
        }

        /// <summary>
        /// Invokes the service factory.
        /// </summary>
        /// <returns>The service instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object InvokeServiceFactory()
        {
            return ServiceInvokerFunc();
        }
    }
}