// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceFactoryBuilder.cs" company="Labo">
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
//   Defines the ServiceFactoryBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// The service factory builder.
    /// </summary>
    internal sealed class ServiceFactoryBuilder : IServiceFactoryBuilder
    {
        /// <summary>
        /// The dynamic assembly builder
        /// </summary>
        private readonly DynamicAssemblyBuilder m_DynamicAssemblyBuilder;

        /// <summary>
        /// The service constructor chooser
        /// </summary>
        private readonly IServiceConstructorChooser m_ServiceConstructorChooser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceFactoryBuilder"/> class.
        /// </summary>
        /// <param name="dynamicAssemblyBuilder">The dynamic assembly builder.</param>
        /// <param name="serviceConstructorChooser">The service constructor chooser.</param>
        public ServiceFactoryBuilder(DynamicAssemblyBuilder dynamicAssemblyBuilder, IServiceConstructorChooser serviceConstructorChooser)
        {
            m_DynamicAssemblyBuilder = dynamicAssemblyBuilder;
            m_ServiceConstructorChooser = serviceConstructorChooser;
        }

        /// <summary>
        /// Builds the service factory.
        /// </summary>
        /// <param name="serviceRegistrationManager">The service registration manager.</param>
        /// <param name="serviceRegistration">The service registration.</param>
        /// <param name="circularDependencyValidator">The circular dependency validator instance.</param>
        /// <returns>Service factory class.</returns>
        public IServiceFactory BuildServiceFactory(IServiceRegistrationManager serviceRegistrationManager, ServiceRegistration serviceRegistration, CircularDependencyValidator circularDependencyValidator)
        {
            circularDependencyValidator.CheckCircularDependency(serviceRegistration.ServiceType);

            if (serviceRegistration.InstanceCreator == null)
            {
                ConstructorInfo constructor = m_ServiceConstructorChooser.GetConstructor(serviceRegistration.ImplementationType);
                ParameterInfo[] constructorParameters = constructor.GetParameters();
                IServiceFactory[] dependentServiceFactories;
                int constructorParametersLength = constructorParameters.Length;
                if (constructorParametersLength == 0)
                {
                    dependentServiceFactories = new IServiceFactory[0];
                }
                else
                {
                    List<IServiceFactory> constructorParameterServiceFactories = new List<IServiceFactory>();
                    for (int i = 0; i < constructorParametersLength; i++)
                    {
                        ParameterInfo constructorParameter = constructorParameters[i];

                        if (serviceRegistrationManager.IsServiceRegistered(constructorParameter.ParameterType))
                        {
                            // TODO: Add dependent service instance creators instead of service factories to eliminate m_ServiceFactory.IsCompiled() check in ServiceInstanceCreator class.
                            IServiceFactory dependentServiceFactory = serviceRegistrationManager.GetServiceCreator(constructorParameter.ParameterType).GetServiceFactory(circularDependencyValidator);
                            constructorParameterServiceFactories.Add(dependentServiceFactory);   
                        }
                    }

                    dependentServiceFactories = constructorParameterServiceFactories.ToArray();
                }

                ServiceFactoryCompilerBase serviceFactoryCompiler;

                // TODO: Service Factory Compiler Factory
                switch (serviceRegistration.ServiceLifetime)
                {
                    case ServiceLifetime.Transient:
                        serviceFactoryCompiler = new TransientServiceFactoryCompiler(m_DynamicAssemblyBuilder, serviceRegistration.ImplementationType, constructor, dependentServiceFactories);
                        break;
                    case ServiceLifetime.Singleton:
                        serviceFactoryCompiler = new SingletonServiceFactoryCompiler(m_DynamicAssemblyBuilder, serviceRegistration.ImplementationType, constructor, dependentServiceFactories);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                circularDependencyValidator.Release();

                IServiceFactory serviceFactory = new ServiceFactory(serviceFactoryCompiler, serviceRegistration.ServiceType);
                serviceFactoryCompiler.SetServiceFactory(serviceFactory);
                return serviceFactory;
            }

            IServiceFactoryInvoker serviceFactoryInvoker = null;

            // TODO: Service Factory Invoker Factory
            switch (serviceRegistration.ServiceLifetime)
            {
                case ServiceLifetime.Transient:
                    serviceFactoryInvoker = new TransientServiceFactoryInvoker(serviceRegistration.InstanceCreator);
                    break;
                case ServiceLifetime.Singleton:
                    serviceFactoryInvoker = new SingletonServiceFactoryInvoker(serviceRegistration.InstanceCreator);
                    break;
            }

            circularDependencyValidator.Release();

            return new ServiceFactory(serviceFactoryInvoker, serviceRegistration.ServiceType);
        }
    }
}
