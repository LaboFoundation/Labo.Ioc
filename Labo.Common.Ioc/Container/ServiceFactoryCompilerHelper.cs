// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceFactoryCompilerHelper.cs" company="Labo">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 Bora Akgun
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
//   Defines the ServiceFactoryCompilerHelper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using Labo.Common.Reflection;

    internal static class ServiceFactoryCompilerHelper
    {
        /// <summary>
        /// Emits the service invoker function.
        /// </summary>
        /// <param name="serviceFactory">The service factory.</param>
        /// <param name="createInstanceMethodIlGenerator">The create instance method utility generator.</param>
        /// <param name="dynamicAssemblyBuilder">The dynamic assembly builder.</param>
        public static void EmitServiceInvokerFunc(IServiceFactory serviceFactory, ILGenerator createInstanceMethodIlGenerator, DynamicAssemblyBuilder dynamicAssemblyBuilder)
        {
            TypeBuilder anonymousTypeBuilder = dynamicAssemblyBuilder.CreateTypeBuilder("AnonymousType_{0}", TypeAttributes.Abstract | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
            anonymousTypeBuilder.DefineField("Func", typeof(Func<object>), FieldAttributes.Public | FieldAttributes.Static);
            Type anonymousType = anonymousTypeBuilder.CreateType();
            FieldInfo funcField = anonymousType.GetField("Func");
            funcField.SetValue(null, serviceFactory.ServiceInvokerFunc);

            EmitHelper.Ldsfld(createInstanceMethodIlGenerator, funcField);
            EmitHelper.CallVirt(createInstanceMethodIlGenerator, typeof(Func<object>).GetMethod("Invoke"));
            EmitHelper.Castclass(createInstanceMethodIlGenerator, serviceFactory.ServiceType);
        }
    }
}
