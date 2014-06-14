// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IServiceRegistrationManager.cs" company="Labo">
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
//   Defines the IServiceRegistrationManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Service registration manager interface.
    /// </summary>
    internal interface IServiceRegistrationManager
    {
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
        /// <returns>
        /// The <see cref="ServiceRegistration"/>.
        /// </returns>
        ServiceRegistration RegisterService(Type serviceType, Type implementationType, ServiceLifetime serviceLifetime, string serviceName = null);

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
        /// <returns>
        /// The <see cref="ServiceRegistration"/>.
        /// </returns>
        ServiceRegistration RegisterService(Type serviceType, Func<object> instanceCreator, ServiceLifetime serviceLifetime, string serviceName = null);

        /// <summary>
        /// Gets the service registration.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Service registration class.</returns>
        ServiceRegistration GetServiceRegistration(Type serviceType, string serviceName);

        /// <summary>
        /// Gets the service registration.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>Service registration class.</returns>
        ServiceRegistration GetServiceRegistration(Type serviceType);

        /// <summary>
        /// Determines whether [is service registered] [the specified service type].
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>true if the service is registered otherwise false.</returns>
        bool IsServiceRegistered(Type serviceType, string serviceName);

        /// <summary>
        /// Determines whether [is service registered] [the specified service type].
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>true if the service is registered otherwise false.</returns>
        bool IsServiceRegistered(Type serviceType);

        /// <summary>
        /// Gets all service creators.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>List of service instance creators.</returns>
        IList<ServiceInstanceCreator> GetAllServiceCreators(Type serviceType);

        /// <summary>
        /// Gets the service creator.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The service instance creator.</returns>
        ServiceInstanceCreator GetServiceCreator(Type serviceType);

        /// <summary>
        /// Gets the service creator.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>The service instance creator.</returns>
        ServiceInstanceCreator GetServiceCreator(Type serviceType, string name);

        /// <summary>
        /// Gets the service invoker.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>Service Invoker</returns>
        Func<object> GetServiceInvoker(Type serviceType);
    }
}