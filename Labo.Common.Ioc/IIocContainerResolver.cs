// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIocContainerResolver.cs" company="Labo">
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
//   Inversion of control container resolver interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Inversion of control container resolver interface.
    /// </summary>
    public interface IIocContainerResolver
    {
        /// <summary>
        /// Determines whether the specified key is registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is registered; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        bool IsRegistered<TService>(string name);

        /// <summary>
        /// Determines whether this instance is registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        ///   <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        bool IsRegistered<TService>();

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered(Type type, string name);

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>all instances.</returns>
        IEnumerable<TService> GetAllInstances<TService>();

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        IEnumerable<object> GetAllInstances(Type serviceType);

        /// <summary>
        /// Gets the instance with parameters.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        TService GetInstance<TService>(object[] parameters) where TService : class;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>instance.</returns>
        TService GetInstance<TService>() where TService : class;

        /// <summary>
        /// Gets the instance by instance name.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The instance name.</param>
        /// <returns>instance.</returns>
        TService GetInstance<TService>(string name);

        /// <summary>
        /// Gets the instance with parameters.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        object GetInstance(Type serviceType, object[] parameters);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instance.</returns>
        object GetInstance(Type serviceType);

        /// <summary>
        /// Gets the instance by name with parameters.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        object GetInstanceByName(Type serviceType, string name, object[] parameters);

        /// <summary>
        /// Gets the instance by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        object GetInstanceByName(Type serviceType, string name);

        /// <summary>
        /// Gets the instance optional with parameters.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        object GetInstanceOptional(Type serviceType, object[] parameters);

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instance.</returns>
        object GetInstanceOptional(Type serviceType);

        /// <summary>
        /// Gets the instance optional with parameters.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        TService GetInstanceOptional<TService>(object[] parameters);

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>instance.</returns>
        TService GetInstanceOptional<TService>();

        /// <summary>
        /// Gets the instance optional by instance name with parameters.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The instance name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        TService GetInstanceOptionalByName<TService>(string name, object[] parameters);

        /// <summary>
        /// Gets the instance optional by instance name.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The instance name.</param>
        /// <returns>instance.</returns>
        TService GetInstanceOptionalByName<TService>(string name);

        /// <summary>
        /// Gets the instance optional by name with parameters.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        object GetInstanceOptionalByName(Type serviceType, string name, object[] parameters);

        /// <summary>
        /// Gets the instance optional by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        object GetInstanceOptionalByName(Type serviceType, string name);
    }
}