// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceProducerContainerFactory.cs" company="Labo">
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
//   Defines the InstanceProducerContainerFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.SimpleInjector
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::SimpleInjector;

    /// <summary>
    /// Instance producer container factory class.
    /// </summary>
    internal sealed class InstanceProducerContainerFactory : SortedList<KeyedService, InstanceProducer>
    {
        /// <summary>
        /// The container
        /// </summary>
        private readonly Container m_Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProducerContainerFactory"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public InstanceProducerContainerFactory(Container container)
        {
            m_Container = container;
        }

        /// <summary>
        /// Registers the specified service type.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="name">The name.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        public void Register(Type serviceType, Type implementationType, string name, Lifestyle lifestyle = null)
        {
            lifestyle = lifestyle ?? Lifestyle.Transient;

            Registration registration = lifestyle.CreateRegistration(serviceType, implementationType, m_Container);

            InstanceProducer producer = new InstanceProducer(serviceType, registration);

            AddServiceInstanceProducer(serviceType, name, producer);
        }

        /// <summary>
        /// Registers the specified instance creator.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="instanceCreator">The instance creator.</param>
        /// <param name="name">The name.</param>
        /// <param name="lifestyle">The lifestyle.</param>
        public void Register<TService>(Func<TService> instanceCreator, string name, Lifestyle lifestyle = null) 
            where TService : class
        {
            lifestyle = lifestyle ?? Lifestyle.Transient;

            Registration registration = lifestyle.CreateRegistration(instanceCreator, m_Container);

            Type serviceType = typeof(TService);
            InstanceProducer producer = new InstanceProducer(serviceType, registration);

            AddServiceInstanceProducer(serviceType, name, producer);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public object GetInstance(Type serviceType, string name)
        {
            KeyedService serviceTypeKey = GetServiceTypeKey(serviceType, name);
            return GetInstance(serviceTypeKey);
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance or null.</returns>
        public object GetInstanceOptional(Type serviceType, string name)
        {
            InstanceProducer instanceProducer;
            if (!TryGetValue(GetServiceTypeKey(serviceType, name), out instanceProducer))
            {
                return null;
            }

            return instanceProducer.GetInstance();
        }

        /// <summary>
        /// Determines whether the specified service type is registered.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns><c>true</c> if is registered else <c>false</c></returns>
        public bool IsRegistered(Type serviceType, string name)
        {
            return ContainsKey(GetServiceTypeKey(serviceType, name));
        }

        /// <summary>
        /// Determines whether the specified service type is registered.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns><c>true</c> if is registered else <c>false</c></returns>
        public bool IsRegistered(Type serviceType)
        {
            return Keys.Any(x => x.ServiceType == serviceType);
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            List<object> instances = new List<object>();
            for (int i = 0; i < Keys.Count; i++)
            {
                KeyedService keyedService = Keys[i];
                if (keyedService.ServiceType == serviceType)
                {
                    instances.Add(GetInstance(keyedService));                    
                }
            }

            return instances;
        }

        /// <summary>
        /// Gets the service type key.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>Service type key.</returns>
        private static KeyedService GetServiceTypeKey(Type serviceType, string name)
        {
            return new KeyedService(name, serviceType);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceTypeKey">The service type key.</param>
        /// <returns>instance.</returns>
        private object GetInstance(KeyedService serviceTypeKey)
        {
            InstanceProducer instanceProducer = this[serviceTypeKey];
            return instanceProducer.GetInstance();
        }

        /// <summary>
        /// Adds the service instance producer.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="producer">The producer.</param>
        private void AddServiceInstanceProducer(Type serviceType, string name, InstanceProducer producer)
        {
            KeyedService serviceTypeKey = GetServiceTypeKey(serviceType, name);

            Add(serviceTypeKey, producer);
        }
    }
}