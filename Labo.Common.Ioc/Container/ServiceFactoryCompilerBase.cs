// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceFactoryCompilerBase.cs" company="Labo">
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
//   Defines the ServiceFactoryCompilerBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    /// <summary>
    /// Service factory compiler base class.
    /// </summary>
    internal abstract class ServiceFactoryCompilerBase : IServiceFactoryCompiler
    {
        /// <summary>
        /// The dependent service factories
        /// </summary>
        protected readonly IServiceFactory[] DependentServiceFactories;

        /// <summary>
        /// The service implementation type
        /// </summary>
        private readonly Type m_ServiceImplementationType;

        /// <summary>
        /// The the parent service factories
        /// </summary>
        private readonly IList<IServiceFactory> m_ParentFactories;

        /// <summary>
        /// The factory type
        /// </summary>
        private Type m_FactoryType;

        /// <summary>
        /// Gets the parent service factories.
        /// </summary>
        /// <value>
        /// The parents.
        /// </value>
        public IList<IServiceFactory> ParentFactories
        {
            get { return m_ParentFactories; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFactoryCompilerBase"/> class.
        /// </summary>
        /// <param name="serviceImplementationType">Type of the service.</param>
        /// <param name="dependentServiceFactories">Dependent service factory compiler</param>
        protected ServiceFactoryCompilerBase(Type serviceImplementationType, params IServiceFactory[] dependentServiceFactories)
        {
            m_ServiceImplementationType = serviceImplementationType;
            m_ParentFactories = new List<IServiceFactory>();

            DependentServiceFactories = dependentServiceFactories;
        }

        /// <summary>
        /// Creates the service factory invoker.
        /// </summary>
        /// <returns>The service factory invoker.</returns>
        public IServiceFactoryInvoker CreateServiceFactoryInvoker()
        {
            CompileServiceFactory();

            return CreateServiceFactoryInvoker(m_FactoryType, m_ServiceImplementationType);
        }

        /// <summary>
        /// Compiles the service factory.
        /// </summary>
        public void CompileServiceFactory()
        {
            if (m_FactoryType == null)
            {
                m_FactoryType = CompileServiceFactoryType();
            }
        }

        /// <summary>
        /// Invalidates this service factory compiler.
        /// </summary>
        public void Invalidate()
        {
            for (int i = 0; i < ParentFactories.Count; i++)
            {
                IServiceFactory parent = ParentFactories[i];
                parent.Invalidate();
            }

            m_FactoryType = null;
        }

        /// <summary>
        /// Emits the service factory creator method.
        /// </summary>
        /// <param name="generator">The utility generator.</param>
        public abstract void EmitServiceFactoryCreatorMethod(ILGenerator generator);

        /// <summary>
        /// Sets the service factory.
        /// </summary>
        /// <param name="serviceFactory">The service factory.</param>
        internal void SetServiceFactory(IServiceFactory serviceFactory)
        {
            for (int i = 0; i < DependentServiceFactories.Length; i++)
            {
                IServiceFactory dependentServiceFactory = DependentServiceFactories[i];
                IServiceFactoryCompiler serviceFactoryCompiler = dependentServiceFactory.ServiceFactoryCompiler;
                if (serviceFactoryCompiler != null)
                {
                    serviceFactoryCompiler.ParentFactories.Add(serviceFactory);                    
                }
            }
        }

        /// <summary>
        /// Compiles the service factory.
        /// </summary>
        /// <returns>
        /// The service factory <see cref="Type"/>.
        /// </returns>
        protected abstract Type CompileServiceFactoryType();

        /// <summary>
        /// Creates the service factory invoker.
        /// </summary>
        /// <param name="factoryType">Type of the factory.</param>
        /// <param name="serviceImplementationType">Type of the service implementation.</param>
        /// <returns>The service factory invoker.</returns>
        protected abstract IServiceFactoryInvoker CreateServiceFactoryInvoker(Type factoryType, Type serviceImplementationType);
    }
}