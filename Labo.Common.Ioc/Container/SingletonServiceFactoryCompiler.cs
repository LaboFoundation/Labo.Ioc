// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingletonServiceFactoryCompiler.cs" company="Labo">
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
//   The singleton service factory compiler class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    /// <summary>
    /// The singleton service factory compiler class.
    /// </summary>
    internal sealed class SingletonServiceFactoryCompiler : ServiceFactoryCompilerBase
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
        /// The create instance method builder
        /// </summary>
        private MethodBuilder m_CreateInstanceMethodBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingletonServiceFactoryCompiler"/> class.
        /// </summary>
        /// <param name="dynamicAssemblyBuilder">The dynamic assembly builder.</param>
        /// <param name="serviceImplementationType">Type of the service.</param>
        /// <param name="serviceConstructor">The service constructor.</param>
        /// <param name="dependentServiceFactories">The dependent service factories.</param>
        public SingletonServiceFactoryCompiler(DynamicAssemblyBuilder dynamicAssemblyBuilder, Type serviceImplementationType, ConstructorInfo serviceConstructor, params IServiceFactory[] dependentServiceFactories)
            : base(serviceImplementationType, dependentServiceFactories)
        {
            m_DynamicAssemblyBuilder = dynamicAssemblyBuilder;
            m_ServiceImplementationType = serviceImplementationType;
            m_ServiceConstructor = serviceConstructor;
        }

        /// <summary>
        /// Emits the service factory creator method.
        /// </summary>
        /// <param name="generator">The utility generator.</param>
        public override void EmitServiceFactoryCreatorMethod(ILGenerator generator)
        {
            EmitHelper.Call(generator, m_CreateInstanceMethodBuilder);
        }

        /// <summary>
        /// Creates the service factory invoker.
        /// </summary>
        /// <param name="factoryType">Type of the factory.</param>
        /// <param name="serviceImplementationType">Type of the service implementation.</param>
        /// <returns>The service factory invoker.</returns>
        protected override IServiceFactoryInvoker CreateServiceFactoryInvoker(Type factoryType, Type serviceImplementationType)
        {
            return new SingletonServiceFactoryInvoker(factoryType, serviceImplementationType);
        }

        /// <summary>
        /// Compiles the service factory.
        /// </summary>
        /// <returns>
        /// The service factory <see cref="Type"/>.
        /// </returns>
        protected override Type CompileServiceFactoryType()
        {
            TypeBuilder typeBuilder = m_DynamicAssemblyBuilder.CreateTypeBuilder("SingletonService_{0}", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
            FieldBuilder singletonFieldBuilder = typeBuilder.DefineField("s_Singleton", m_ServiceImplementationType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);

            EmitStaticConstructor(typeBuilder, singletonFieldBuilder);

            m_CreateInstanceMethodBuilder = typeBuilder.DefineMethod("CreateInstance", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static, m_ServiceImplementationType, Type.EmptyTypes);
            ILGenerator createInstanceMethodIlGenerator = m_CreateInstanceMethodBuilder.GetILGenerator();

            EmitHelper.Ldsfld(createInstanceMethodIlGenerator, singletonFieldBuilder);
            EmitHelper.Ret(createInstanceMethodIlGenerator);

            return typeBuilder.CreateType();
        }

        /// <summary>
        /// Emits the static constructor of the service factory type.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="singletonFieldBuilder">The singleton field builder.</param>
        private void EmitStaticConstructor(TypeBuilder typeBuilder, FieldBuilder singletonFieldBuilder)
        {
            ConstructorBuilder staticConstructorBuilder = typeBuilder.DefineTypeInitializer();
            ILGenerator staticConstructorIlGenerator = staticConstructorBuilder.GetILGenerator();

            for (int i = 0; i < DependentServiceFactories.Length; i++)
            {
                IServiceFactory dependentServiceFactory = DependentServiceFactories[i];
                IServiceFactoryCompiler dependentServiceFactoryCompiler = dependentServiceFactory.ServiceFactoryCompiler;
                if (dependentServiceFactoryCompiler != null)
                {
                    dependentServiceFactoryCompiler.CompileServiceFactory();
                    dependentServiceFactoryCompiler.EmitServiceFactoryCreatorMethod(staticConstructorIlGenerator);
                }
                else
                {
                    ServiceFactoryCompilerHelper.EmitServiceInvokerFunc(dependentServiceFactory, staticConstructorIlGenerator, m_DynamicAssemblyBuilder);
                }
            }

            EmitHelper.Newobj(staticConstructorIlGenerator, m_ServiceConstructor);
            EmitHelper.Stsfld(staticConstructorIlGenerator, singletonFieldBuilder);
            EmitHelper.Ret(staticConstructorIlGenerator);
        }
    }
}