// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceFactory.cs" company="Labo">
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
//   Defines the ServiceFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The service factory class.
    /// </summary>
    internal sealed class ServiceFactory : IServiceFactory
    {
        /// <summary>
        /// The service factory invoker
        /// </summary>
        private IServiceFactoryInvoker m_ServiceFactoryInvoker;

        /// <summary>
        /// Gets the service factory compiler.
        /// </summary>
        /// <value>
        /// The service factory compiler.
        /// </value>
        public IServiceFactoryCompiler ServiceFactoryCompiler { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFactory"/> class.
        /// </summary>
        /// <param name="serviceFactoryInvoker">The service factory invoker.</param>
        public ServiceFactory(IServiceFactoryInvoker serviceFactoryInvoker)
        {
            m_ServiceFactoryInvoker = serviceFactoryInvoker;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFactory"/> class.
        /// </summary>
        /// <param name="serviceFactoryCompiler">The service factory compiler.</param>
        public ServiceFactory(IServiceFactoryCompiler serviceFactoryCompiler)
        {
            ServiceFactoryCompiler = serviceFactoryCompiler;

            m_ServiceFactoryInvoker = ServiceFactoryCompiler.CreateServiceFactoryInvoker();
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The service instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetServiceInstance(object[] parameters)
        {
            return m_ServiceFactoryInvoker.InvokeServiceFactory(parameters);
        }

        /// <summary>
        /// Gets the service instance.
        /// </summary>
        /// <returns>The service instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public object GetServiceInstance()
        {
            return m_ServiceFactoryInvoker.InvokeServiceFactory();
        }

        /// <summary>
        /// Invalidates this service factory.
        /// </summary>
        public void Invalidate()
        {
            ServiceFactoryCompiler.Invalidate();
            ServiceFactoryCompiler = null;
            m_ServiceFactoryInvoker = null;
        }

        /// <summary>
        /// Determines whether the factory is compiled.
        /// </summary>
        /// <returns>returns <c>true</c> if factory is compiled, otherwise <c>false</c></returns>
        public bool IsCompiled()
        {
            return m_ServiceFactoryInvoker != null;
        }
    }
}
