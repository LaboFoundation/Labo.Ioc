// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDynamicAssemblyManager.cs" company="Labo">
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
//   Defines the IDynamicAssemblyManager type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// Dynamic assembly manager interface.
    /// </summary>
    public interface IDynamicAssemblyManager
    {
        /// <summary>
        /// Defines the dynamic assembly.
        /// </summary>
        /// <returns>Assembly builder.</returns>
        AssemblyBuilder DefineDynamicAssembly();

        /// <summary>
        /// Defines the module builder.
        /// </summary>
        /// <param name="assemblyBuilder">The assembly builder.</param>
        /// <returns>Module builder.</returns>
        /// <exception cref="System.ArgumentNullException">assemblyBuilder</exception>
        ModuleBuilder DefineModuleBuilder(AssemblyBuilder assemblyBuilder);

        /// <summary>
        /// Defines the type builder.
        /// </summary>
        /// <param name="moduleBuilder">The module builder.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>Type builder.</returns>
        /// <exception cref="System.ArgumentNullException">moduleBuilder</exception>
        TypeBuilder DefineTypeBuilder(ModuleBuilder moduleBuilder, string className, TypeAttributes attributes);
    }
}