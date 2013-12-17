// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NInjectIocContainer.cs" company="Labo">
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
//   NInject inversion of control container class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.NInject
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using global::Ninject;

    using Ninject.Parameters;

    /// <summary>
    /// NInject inversion of control container class.
    /// </summary>
    public sealed class NInjectIocContainer : BaseIocContainer
    {
        /// <summary>
        /// The container
        /// </summary>
        private readonly IKernel m_Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="NInjectIocContainer"/> class.
        /// </summary>
        public NInjectIocContainer()
        {
            m_Container = new StandardKernel();
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        public override void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            m_Container.Bind<TImplementation>().ToMethod(x => creator(this)).InSingletonScope();
        }

        /// <summary>
        /// Registers the single instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            m_Container.Bind<TImplementation>().ToMethod(x => creator(this)).InSingletonScope().Named(name);
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="implementationType">The type of the implementation.</param>
        public override void RegisterSingleInstance(Type serviceType, Type implementationType)
        {
            m_Container.Bind(serviceType).To(implementationType).InSingletonScope();
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
            m_Container.Bind(serviceType).To(implementationType).InSingletonScope().Named(name);
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
            m_Container.Bind(serviceType).To(serviceType).InSingletonScope().Named(name);
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
            m_Container.Bind<TImplementation>().ToMethod(x => creator(this)).InTransientScope();
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
            m_Container.Bind(serviceType).To(implementationType).InTransientScope();
        }

        /// <summary>
        /// Registers the instance named.
        /// </summary>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <param name="creator">The creator delegate.</param>
        /// <param name="name">The instance name.</param>
        public override void RegisterInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            m_Container.Bind<TImplementation>().ToMethod(x => creator(this)).InTransientScope().Named(name);
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
            m_Container.Bind(serviceType).To(implementationType).InTransientScope().Named(name);
        }

        /// <summary>
        /// The register ınstance named.
        /// </summary>
        /// <param name="serviceType">
        /// The service type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public override void RegisterInstanceNamed(Type serviceType, string name)
        {
            m_Container.Bind(serviceType).To(serviceType).InTransientScope().Named(name);
        }

        /// <summary>
        /// Registers the single instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterSingleInstance(Type serviceType)
        {
            m_Container.Bind(serviceType).To(serviceType).InSingletonScope();
        }

        /// <summary>
        /// Registers the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        public override void RegisterInstance(Type serviceType)
        {
            m_Container.Bind(serviceType).To(serviceType).InTransientScope();
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstance(Type serviceType, object[] parameters)
        {
            return m_Container.Get(serviceType, GetNinjectParameters(parameters));
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instance.</returns>
        public override object GetInstance(Type serviceType)
        {
            return m_Container.Get(serviceType, GetNinjectParameters());
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
            return m_Container.Get(serviceType, name, GetNinjectParameters(parameters));
        }

        /// <summary>
        /// Gets the instance by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceByName(Type serviceType, string name)
        {
            return m_Container.Get(serviceType, name, GetNinjectParameters());
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptional(Type serviceType, object[] parameters)
        {
            return m_Container.TryGet(serviceType, GetNinjectParameters(parameters));
        }

        /// <summary>
        /// Gets the instance optional.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptional(Type serviceType)
        {
            return m_Container.TryGet(serviceType, GetNinjectParameters());
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
            return m_Container.TryGet(serviceType, name, GetNinjectParameters(parameters));
        }

        /// <summary>
        /// Gets the instance optional by name.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="name">The name.</param>
        /// <returns>instance.</returns>
        public override object GetInstanceOptionalByName(Type serviceType, string name)
        {
            return m_Container.TryGet(serviceType, name, GetNinjectParameters());
        }

        /// <summary>
        /// Gets all instances.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>all instances.</returns>
        public override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return m_Container.GetAll(serviceType);
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
            return m_Container.GetBindings(type).Any();
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
            return m_Container.GetBindings(type).Any(x => x.Metadata.Name == name);
        }

        /// <summary>
        /// Gets the ninject parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>parameters.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static IParameter[] GetNinjectParameters(params object[] parameters)
        {
            IParameter[] autofacParameters = new IParameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                object parameter = parameters[i];
                autofacParameters[i] = new ConstructorArgument(string.Empty, parameter);
            }

            return autofacParameters;
        }
    }
}
