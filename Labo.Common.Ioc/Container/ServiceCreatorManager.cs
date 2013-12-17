// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceCreatorManager.cs" company="Labo">
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
//   Defines the ServiceCreatorManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    using Labo.Common.Ioc.Container.Exceptions;
    using Labo.Common.Ioc.Resources;

    /// <summary>
    /// The service creator manager class.
    /// </summary>
    internal sealed class ServiceCreatorManager : IServiceCreatorManager
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
        /// The service creator entries
        /// </summary>
        private readonly Dictionary<ServiceKey, ServiceInstanceCreator> m_ServiceCreatorEntries;

        /// <summary>
        /// The service creator entries by service type
        /// </summary>
        private readonly Dictionary<Type, ServiceInstanceCreator> m_ServiceCreatorEntriesByServiceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCreatorManager"/> class.
        /// </summary>
        /// <param name="serviceRegistrationManager">The service registration manager.</param>
        /// <param name="serviceFactoryBuilder">The service factory builder.</param>
        public ServiceCreatorManager(IServiceRegistrationManager serviceRegistrationManager, IServiceFactoryBuilder serviceFactoryBuilder)
        {
            m_ServiceRegistrationManager = serviceRegistrationManager;
            m_ServiceFactoryBuilder = serviceFactoryBuilder;

            m_ServiceCreatorEntries = new Dictionary<ServiceKey, ServiceInstanceCreator>(32);
            m_ServiceCreatorEntriesByServiceType = new Dictionary<Type, ServiceInstanceCreator>(32);
        }

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <param name="serviceType">
        /// Type of the service.
        /// </param>
        /// <param name="implementationType">
        /// Type of the implementation.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime.
        /// </param>
        /// <param name="serviceName">
        /// Name of the service.
        /// </param>
        public void RegisterService(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime, string serviceName = null)
        {
            RegisterService(serviceType, serviceName, m_ServiceRegistrationManager.RegisterService(serviceType, implementationType, serviceLifetime, serviceName));
        }

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="instanceCreator">
        /// The instance creator.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime.
        /// </param>
        /// <param name="serviceName">
        /// Name of the service.
        /// </param>
        public void RegisterService(Type serviceType, Func<object> instanceCreator, ServiceLifetime serviceLifetime, string serviceName = null)
        {
            RegisterService(serviceType, serviceName, m_ServiceRegistrationManager.RegisterService(serviceType, instanceCreator, serviceLifetime, serviceName));
        }

        /// <summary>
        /// Gets the service creator.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>ServiceInstanceCreator class.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ServiceInstanceCreator GetServiceCreator(Type serviceType, string serviceName)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            ServiceInstanceCreator serviceCreator;
            if (!m_ServiceCreatorEntries.TryGetValue(new ServiceKey(serviceName, serviceType), out serviceCreator))
            {
                throw new IocContainerRegistrationException(string.Format(CultureInfo.CurrentCulture, Strings.ServiceCreatorManager_GetServiceCreator_No_registration_found_for_type_and_name, serviceType, serviceName));
            }

            return serviceCreator;
        }

        /// <summary>
        /// Gets the service creator.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>ServiceInstanceCreator class.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ServiceInstanceCreator GetServiceCreator(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            ServiceInstanceCreator serviceCreator;
            if (!m_ServiceCreatorEntriesByServiceType.TryGetValue(serviceType, out serviceCreator))
            {
                throw new IocContainerRegistrationException(string.Format(CultureInfo.CurrentCulture, Strings.ServiceCreatorManager_GetServiceCreator_No_registration_could_be_found_for_type, serviceType));
            }

            return serviceCreator;
        }

        /// <summary>
        /// Gets all service creators.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The list of service creators.</returns>
        public IList<ServiceInstanceCreator> GetAllServiceCreators(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            List<ServiceInstanceCreator> instances = new List<ServiceInstanceCreator>();
            Dictionary<ServiceKey, ServiceInstanceCreator>.Enumerator serviceEntriesEnumerator = m_ServiceCreatorEntries.GetEnumerator();
            while (serviceEntriesEnumerator.MoveNext())
            {
                KeyValuePair<ServiceKey, ServiceInstanceCreator> entry = serviceEntriesEnumerator.Current;
                if (entry.Key.ServiceType == serviceType)
                {
                    instances.Add(entry.Value);
                }
            }

            Dictionary<Type, ServiceInstanceCreator>.Enumerator serviceEntriesByKeyEnumerator = m_ServiceCreatorEntriesByServiceType.GetEnumerator();
            while (serviceEntriesByKeyEnumerator.MoveNext())
            {
                KeyValuePair<Type, ServiceInstanceCreator> entry = serviceEntriesByKeyEnumerator.Current;
                if (entry.Key == serviceType)
                {
                    instances.Add(entry.Value);
                }
            }

            return instances;
        }

        /// <summary>
        /// Registers the service.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="serviceRegistration">The service registration.</param>
        private void RegisterService(Type serviceType, string serviceName, ServiceRegistration serviceRegistration)
        {
            // TODO: Recompile service factory when existing service registration changes.

            if (serviceName == null)
            {
                m_ServiceCreatorEntriesByServiceType[serviceType] = new ServiceInstanceCreator(m_ServiceFactoryBuilder, serviceRegistration);
            }
            else
            {
                m_ServiceCreatorEntries[new ServiceKey(serviceName, serviceType)] = new ServiceInstanceCreator(m_ServiceFactoryBuilder, serviceRegistration);
            }
        }
    }
}
