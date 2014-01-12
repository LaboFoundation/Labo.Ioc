// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceRegistration.cs" company="Labo">
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
//   Defines the ServiceRegistration type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;

    /// <summary>
    /// Service registration class.
    /// </summary>
    internal sealed class ServiceRegistration
    {
        /// <summary>
        /// Gets or sets the service lifetime.
        /// </summary>
        /// <value>
        /// The service lifetime.
        /// </value>
        public ServiceLifetime ServiceLifetime { get; set; }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets the type of the implementation.
        /// </summary>
        /// <value>
        /// The type of the implementation.
        /// </value>
        public Type ImplementationType { get; private set; }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; private set; }

        /// <summary>
        /// Gets the instance creator.
        /// </summary>
        /// <value>
        /// The instance creator.
        /// </value>
        public Func<object> InstanceCreator { get; private set; }

        /// <summary>
        /// Gets the service instance creator.
        /// </summary>
        /// <value>
        /// The service instance creator.
        /// </value>
        public ServiceInstanceCreator ServiceInstanceCreator { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistration"/> class.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="instanceCreator">The instance creator.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        /// <param name="serviceName">Name of the service.</param>
        public ServiceRegistration(Type serviceType, Func<object> instanceCreator, ServiceLifetime serviceLifetime, string serviceName = null)
        {
            ServiceType = serviceType;
            InstanceCreator = instanceCreator;
            ServiceLifetime = serviceLifetime;
            ServiceName = serviceName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRegistration"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implementationType">Type of the implementation.</param>
        /// <param name="serviceLifetime">The service lifetime.</param>
        /// <param name="serviceName">Name of the service.</param>
        public ServiceRegistration(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime, string serviceName = null)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            ServiceLifetime = serviceLifetime;
            ServiceName = serviceName;
        }

        /// <summary>
        /// Sets the service instance creator.
        /// </summary>
        /// <param name="serviceInstanceCreator">The service instance creator.</param>
        internal void SetServiceInstanceCreator(ServiceInstanceCreator serviceInstanceCreator)
        {
            ServiceInstanceCreator = serviceInstanceCreator;
        }
    }
}
