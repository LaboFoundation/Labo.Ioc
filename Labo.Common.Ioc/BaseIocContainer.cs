// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseIocContainer.cs" company="Labo">
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
//   Defines the BaseIocContainer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Base inversion of control container.
    /// </summary>
    public abstract class BaseIocContainer : IIocContainer
    {
        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        public abstract void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator) where TImplementation : class;

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public abstract void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name) where TImplementation : class;

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public abstract void RegisterSingleInstance(Type serviceType);

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public abstract void RegisterInstance(Type serviceType);

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        public abstract void RegisterSingleInstance(Type serviceType, Type implementationType);

        /// <summary>
        /// The register singleton named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public abstract void RegisterSingleInstanceNamed(Type serviceType, Type implementationType, string name);

        /// <summary>
        /// The register singleton named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public abstract void RegisterSingleInstanceNamed(Type serviceType, string name);

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="creator">
        /// The creator.
        /// </param>
        /// <typeparam name="TImplementation">
        /// The implementation type.
        /// </typeparam>
        public abstract void RegisterInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator) where TImplementation : class;

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        public abstract void RegisterInstance(Type serviceType, Type implementationType);

        /// <summary>
        /// Registers the instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public abstract void RegisterInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name) where TImplementation : class;

        /// <summary>
        /// The register named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public abstract void RegisterInstanceNamed(Type serviceType, Type implementationType, string name);

        /// <summary>
        /// The register singleton named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public abstract void RegisterInstanceNamed(Type serviceType, string name);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public abstract object GetInstance(Type serviceType, object[] parameters);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instance.</returns>
        public abstract object GetInstance(Type serviceType);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public abstract object GetInstanceByName(Type serviceType, string name, object[] parameters);

        /// <summary>
        /// Gets the instance by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public abstract object GetInstanceByName(Type serviceType, string name);

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public abstract object GetInstanceOptional(Type serviceType, object[] parameters);

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instance.</returns>
        public abstract object GetInstanceOptional(Type serviceType);

        /// <summary>
        /// Gets the instance optional by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public abstract object GetInstanceOptionalByName(Type serviceType, string name, object[] parameters);

        /// <summary>
        /// Gets the instance optional by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public abstract object GetInstanceOptionalByName(Type serviceType, string name);

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        public abstract IEnumerable<object> GetAllInstances(Type serviceType);

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsRegistered(Type type);

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool IsRegistered(Type type, string name);

        /// <summary>
        /// Determines whether the specified key is registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegistered<TService>(string name)
        {
            return IsRegistered(typeof(TService), name);
        }

        /// <summary>
        /// Determines whether this instance is registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        ///   <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRegistered<TService>()
        {
            return IsRegistered(typeof(TService));
        }

        /// <summary>
        /// Gets the instance with parameters.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public TService GetInstance<TService>(object[] parameters)
              where TService : class 
        {
            return (TService)GetInstance(typeof(TService), parameters);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>instance.</returns>
        public TService GetInstance<TService>()
            where TService : class
        {
            return (TService)GetInstance(typeof(TService));
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>all instances.</returns>
        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return GetAllInstances(typeof(TService)).Cast<TService>();
        }

        /// <summary>
        /// Gets the instance by instance name.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The instance name.</param>
        /// <returns>instance.</returns>
        public TService GetInstance<TService>(string name)
        {
            return (TService)GetInstanceByName(typeof(TService), name);
        }

        /// <summary>
        /// Registers the module.
        /// </summary>
        /// <param name="iocModule">The module.</param>
        public void RegisterModule(IIocModule iocModule)
        {
            if (iocModule == null)
            {
                throw new ArgumentNullException("iocModule");
            }

            iocModule.Configure(this);
        }

        /// <summary>
        /// Registers the named instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="name">The instance name.</param>
        public void RegisterInstanceNamed<TService, TImplementation>(string name) where TImplementation : TService
        {
            RegisterInstanceNamed(typeof(TService), typeof(TImplementation), name);
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void RegisterInstance<TService, TImplementation>() where TImplementation : TService
        {
            RegisterInstance(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="name">The instance name.</param>
        public void RegisterSingleInstanceNamed<TService, TImplementation>(string name) where TImplementation : TService
        {
            RegisterSingleInstanceNamed(typeof(TService), typeof(TImplementation), name);
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        public void RegisterSingleInstance<TService, TImplementation>()
            where TImplementation : TService
        {
            RegisterSingleInstance(typeof(TService), typeof(TImplementation));
        }

        /// <summary>
        /// Gets the instance optional with parameters.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public TService GetInstanceOptional<TService>(object[] parameters)
        {
            return (TService)GetInstanceOptional(typeof(TService), parameters);
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>instance.</returns>
        public TService GetInstanceOptional<TService>()
        {
            return (TService)GetInstanceOptional(typeof(TService));
        }

        /// <summary>
        /// Gets the instance optional by instance name.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The instance name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public TService GetInstanceOptionalByName<TService>(string name, object[] parameters)
        {
            return (TService)GetInstanceOptionalByName(typeof(TService), name, parameters);
        }

        /// <summary>
        /// Gets the instance optional by instance name.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="name">The instance name.</param>
        /// <returns>instance.</returns>
        public TService GetInstanceOptionalByName<TService>(string name)
        {
            return (TService)GetInstanceOptionalByName(typeof(TService), name);
        }
    }
}
