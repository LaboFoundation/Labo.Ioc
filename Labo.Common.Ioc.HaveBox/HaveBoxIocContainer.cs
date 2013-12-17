// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HaveBoxIocContainer.cs" company="Labo">
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
//   HaveBox inversion of control container class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.HaveBox
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using global::HaveBox;

    /// <summary>
    /// HaveBox inversion of control container class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    public sealed class HaveBoxIocContainer : BaseIocContainer
    {
        /// <summary>
        /// The container
        /// </summary>
        private readonly Container m_Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="HaveBoxIocContainer"/> class.
        /// </summary>
        public HaveBoxIocContainer()
        {
            m_Container = new Container();
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        public override void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            Lazy<TImplementation> serviceCreator = new Lazy<TImplementation>(() => creator(this), true);
            m_Container.Configure(x => x.For<TImplementation>().Use(() => serviceCreator.Value));
        }

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        public override void RegisterSingleInstance(Type serviceType, Type implementationType)
        {
            m_Container.Configure(x => x.For(serviceType).Use(implementationType).AsSingleton());
        }

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
        public override void RegisterSingleInstanceNamed(Type serviceType, Type implementationType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The register singleton named instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public override void RegisterSingleInstanceNamed(Type serviceType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="creator">
        /// The creator.
        /// </param>
        /// <typeparam name="TImplementation">
        /// The implementation type.
        /// </typeparam>
        public override void RegisterInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            m_Container.Configure(x => x.For<TImplementation>().Use(() => creator(this)));
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="implementationType">
        /// The implementation type.
        /// </param>
        public override void RegisterInstance(Type serviceType, Type implementationType)
        {
            m_Container.Configure(x => x.For(serviceType).Use(implementationType));
        }

        /// <summary>
        /// Registers the instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            throw new NotImplementedException();
        }

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
        public override void RegisterInstanceNamed(Type serviceType, Type implementationType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The register instance named.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public override void RegisterInstanceNamed(Type serviceType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterSingleInstance(Type serviceType)
        {
            m_Container.Configure(x => x.For(serviceType).Use(serviceType).AsSingleton());
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterInstance(Type serviceType)
        {
            m_Container.Configure(x => x.For(serviceType).Use(serviceType));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstance(Type serviceType, object[] parameters)
        {
            return m_Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instance.</returns>
        public override object GetInstance(Type serviceType)
        {
            return m_Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceByName(Type serviceType, string name, object[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the instance by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceByName(Type serviceType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptional(Type serviceType, object[] parameters)
        {
            return m_Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptional(Type serviceType)
        {
            return m_Container.GetInstance(serviceType);
        }

        /// <summary>
        /// Gets the instance optional by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptionalByName(Type serviceType, string name, object[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the instance optional by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptionalByName(Type serviceType, string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        public override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRegistered(Type type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the specified type is registered.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is registered; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsRegistered(Type type, string name)
        {
            throw new NotImplementedException();
        }
    }
}
