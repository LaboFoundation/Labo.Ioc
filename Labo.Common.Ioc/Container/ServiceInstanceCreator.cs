// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceInstanceCreator.cs" company="Labo">
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
//   The service instance creator class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The service instance creator class.
    /// </summary>
    internal sealed class ServiceInstanceCreator
    {
        /// <summary>
        /// The service registration manager
        /// </summary>
        private readonly IServiceRegistrationManager m_ServiceRegistrationManager;

        /// <summary>
        /// The service factory builder
        /// </summary>
        private readonly IServiceFactoryBuilder m_ServiceFactoryBuilder;
        
        /// <summary>
        /// The service registration
        /// </summary>
        private readonly ServiceRegistration m_ServiceRegistration;

        /// <summary>
        /// On service factory invalidated action.
        /// </summary>
        private readonly Action<Type> m_OnServiceFactoryInvalidated;

        /// <summary>
        /// The service factory
        /// </summary>
        private IServiceFactory m_ServiceFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInstanceCreator"/> class.
        /// </summary>
        /// <param name="serviceRegistrationManager">The service registration manager.</param>
        /// <param name="serviceFactoryBuilder">The service factory builder.</param>
        /// <param name="serviceRegistration">The service registration.</param>
        /// <param name="onServiceFactoryInvalidated">On service factory invalidated action.</param>
        public ServiceInstanceCreator(IServiceRegistrationManager serviceRegistrationManager, IServiceFactoryBuilder serviceFactoryBuilder, ServiceRegistration serviceRegistration, Action<Type> onServiceFactoryInvalidated)
        {
            m_ServiceRegistrationManager = serviceRegistrationManager;
            m_ServiceFactoryBuilder = serviceFactoryBuilder;
            m_ServiceRegistration = serviceRegistration;
            m_OnServiceFactoryInvalidated = onServiceFactoryInvalidated;
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The service instance.</returns>
#if net45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public object GetServiceInstance(object[] parameters)
        {
            return GetServiceFactory().GetServiceInstance(parameters);
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <returns>The service instance.</returns>
#if net45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public object GetServiceInstance()
        {
            return GetServiceFactory().GetServiceInstance();
        }

        /// <summary>
        /// Invalidates the service instance creator.
        /// </summary>
        public void Invalidate()
        {
            if (m_ServiceFactory != null)
            {
                m_ServiceFactory.Invalidate();
                m_ServiceFactory = null;
            }
        }

        /// <summary>
        /// Gets the service factory.
        /// </summary>
        /// <returns>
        /// The service factory.
        /// </returns>
#if net45
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public IServiceFactory GetServiceFactory()
        {
            if (m_ServiceFactory != null && m_ServiceFactory.IsCompiled())
            {
                return m_ServiceFactory;
            }

            using (CircularDependencyValidator circularDependencyValidator = new CircularDependencyValidator())
            {
                InvokeServiceFactory(circularDependencyValidator);
                return m_ServiceFactory;
            }
        }

        /// <summary>
        /// Gets the service factory.
        /// </summary>
        /// <param name="circularDependencyValidator">The circular dependency validator.</param>
        /// <returns>
        /// The service factory.
        /// </returns>
        internal IServiceFactory GetServiceFactory(CircularDependencyValidator circularDependencyValidator)
        {
            if (m_ServiceFactory != null && m_ServiceFactory.IsCompiled())
            {
                return m_ServiceFactory;
            }

            InvokeServiceFactory(circularDependencyValidator);
            return m_ServiceFactory;
        }

        /// <summary>
        /// Invokes the service factory.
        /// </summary>
        /// <param name="circularDependencyValidator">The circular dependency validator.</param>
        private void InvokeServiceFactory(CircularDependencyValidator circularDependencyValidator)
        {
            m_ServiceFactory = m_ServiceFactoryBuilder.BuildServiceFactory(m_ServiceRegistrationManager, m_ServiceRegistration, circularDependencyValidator);
            m_ServiceFactory.OnInvalidated += (sender, args) =>
            {
                if (m_OnServiceFactoryInvalidated != null)
                {
                    m_OnServiceFactoryInvalidated(args.ServiceType);
                }
            };
        }
    }
}