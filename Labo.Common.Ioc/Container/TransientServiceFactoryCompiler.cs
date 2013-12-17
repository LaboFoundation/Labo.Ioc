// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransientServiceFactoryCompiler.cs" company="Labo">
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
//   Defines the TransientServiceFactoryCompiler type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    /// <summary>
    /// The transient service factory compiler class.
    /// </summary>
    internal sealed class TransientServiceFactoryCompiler : IServiceFactoryCompiler
    {
        /// <summary>
        /// The dynamic assembly builder
        /// </summary>
        private readonly DynamicAssemblyBuilder m_DynamicAssemblyBuilder;

        /// <summary>
        /// The service implementation type
        /// </summary>
        private readonly Type m_ServiceImplementationType;

        /// <summary>
        /// The service constructor
        /// </summary>
        private readonly ConstructorInfo m_ServiceConstructor;

        /// <summary>
        /// The dependent service factory compilers
        /// </summary>
        private readonly IServiceFactoryCompiler[] m_DependentServiceFactoryCompilers;

        /// <summary>
        /// The factory type
        /// </summary>
        private Type m_FactoryType;

        /// <summary>
        /// The create instance method builder
        /// </summary>
        private MethodBuilder m_CreateInstanceMethodBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientServiceFactoryCompiler"/> class.
        /// </summary>
        /// <param name="dynamicAssemblyBuilder">The dynamic assembly builder.</param>
        /// <param name="serviceImplementationType">Type of the service.</param>
        /// <param name="serviceConstructor">The service constructor.</param>
        /// <param name="dependentServiceFactoryCompilers">The dependent service factory compilers.</param>
        public TransientServiceFactoryCompiler(DynamicAssemblyBuilder dynamicAssemblyBuilder, Type serviceImplementationType, ConstructorInfo serviceConstructor, params IServiceFactoryCompiler[] dependentServiceFactoryCompilers)
        {
            m_DynamicAssemblyBuilder = dynamicAssemblyBuilder;
            m_ServiceImplementationType = serviceImplementationType;
            m_ServiceConstructor = serviceConstructor;
            m_DependentServiceFactoryCompilers = dependentServiceFactoryCompilers;
        }

        /// <summary>
        /// Creates the service factory invoker.
        /// </summary>
        /// <returns>The service factory invoker.</returns>
        public IServiceFactoryInvoker CreateServiceFactoryInvoker()
        {
            CompileServiceFactory();

            return new TransientServiceFactoryInvoker(m_FactoryType, m_ServiceImplementationType);
        }

        /// <summary>
        /// Compiles the service factory.
        /// </summary>
        public void CompileServiceFactory()
        {
            if (m_FactoryType == null)
            {
                TypeBuilder typeBuilder = m_DynamicAssemblyBuilder.CreateTypeBuilder("TransientService_{0}", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                m_CreateInstanceMethodBuilder = typeBuilder.DefineMethod("CreateInstance", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static, m_ServiceImplementationType, Type.EmptyTypes);
                ILGenerator createInstanceMethodIlGenerator = m_CreateInstanceMethodBuilder.GetILGenerator();

                for (int i = 0; i < m_DependentServiceFactoryCompilers.Length; i++)
                {
                    IServiceFactoryCompiler dependentServiceFactoryCompiler = m_DependentServiceFactoryCompilers[i];
                    dependentServiceFactoryCompiler.CompileServiceFactory();
                    dependentServiceFactoryCompiler.EmitServiceFactoryCreatorMethod(createInstanceMethodIlGenerator);
                }

                EmitHelper.Newobj(createInstanceMethodIlGenerator, m_ServiceConstructor);
                EmitHelper.Ret(createInstanceMethodIlGenerator);

                m_FactoryType = typeBuilder.CreateType();
            }
        }

        /// <summary>
        /// Emits the service factory creator method.
        /// </summary>
        /// <param name="generator">The utility generator.</param>
        public void EmitServiceFactoryCreatorMethod(ILGenerator generator)
        {
            EmitHelper.Call(generator, m_CreateInstanceMethodBuilder);
        }
    }
}
