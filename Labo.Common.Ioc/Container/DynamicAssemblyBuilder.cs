// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DynamicAssemblyBuilder.cs" company="Labo">
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
//   Defines the DynamicAssemblyBuilder type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System.Globalization;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;

    /// <summary>
    /// Dynamic assembly builder class.
    /// </summary>
    internal sealed class DynamicAssemblyBuilder
    {
        /// <summary>
        /// The type unique identifier counter
        /// </summary>
        private static long s_TypeIdCounter;

        /// <summary>
        /// The dynamic assembly manager
        /// </summary>
        private readonly IDynamicAssemblyManager m_DynamicAssemblyManager;

        /// <summary>
        /// The assembly builder
        /// </summary>
        private readonly AssemblyBuilder m_AssemblyBuilder;

        /// <summary>
        /// The module builder
        /// </summary>
        private readonly ModuleBuilder m_ModuleBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicAssemblyBuilder"/> class.
        /// </summary>
        /// <param name="dynamicAssemblyManager">The dynamic assembly manager.</param>
        public DynamicAssemblyBuilder(IDynamicAssemblyManager dynamicAssemblyManager)
        {
            m_DynamicAssemblyManager = dynamicAssemblyManager;

            m_AssemblyBuilder = m_DynamicAssemblyManager.DefineDynamicAssembly();
            m_ModuleBuilder = m_DynamicAssemblyManager.DefineModuleBuilder(m_AssemblyBuilder);
        }

        /// <summary>
        /// Gets the module builder.
        /// </summary>
        /// <value>
        /// The module builder.
        /// </value>
        public ModuleBuilder ModuleBuilder
        {
            get
            {
                return m_ModuleBuilder;
            }
        }

        /// <summary>
        /// Creates type builder.
        /// </summary>
        /// <param name="typeNameFormat">The type name format.</param>
        /// <param name="typeAttributes">The type attributes.</param>
        /// <returns>The created type builder.</returns>
        public TypeBuilder CreateTypeBuilder(string typeNameFormat, TypeAttributes typeAttributes)
        {
            long typeId = Interlocked.Increment(ref s_TypeIdCounter);
            return ModuleBuilder.DefineType(string.Format(CultureInfo.InvariantCulture, typeNameFormat, typeId), typeAttributes);
        }
    }
}
