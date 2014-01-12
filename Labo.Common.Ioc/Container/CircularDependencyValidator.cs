// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CircularDependencyValidator.cs" company="Labo">
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
//   Circular dependency validator class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Labo.Common.Ioc.Container.Exceptions;
    using Labo.Common.Ioc.Resources;

    /// <summary>
    /// Circular dependency validator class.
    /// </summary>
    internal sealed class CircularDependencyValidator : IDisposable
    {
        /// <summary>
        /// The maximum service resolve depth
        /// </summary>
        private const int MAX_RESOLVE_DEPTH = 50;

        /// <summary>
        /// The type to validate circular dependency.
        /// </summary>
        private Stack<Type> m_TypeToValidateStack = new Stack<Type>();

        /// <summary>
        /// The disposed flag.
        /// </summary>
        private bool m_Disposed;

        /// <summary>
        /// Checks the circular dependency.
        /// </summary>
        /// <param name="typeToValidate">
        /// The type To Validate.
        /// </param>
        /// <exception cref="IocContainerDependencyResolutionException">
        /// thrown when circular dependency detected.
        /// </exception>
        public void CheckCircularDependency(Type typeToValidate)
        {
            lock (this)
            {
                if (m_TypeToValidateStack.Count >= MAX_RESOLVE_DEPTH)
                {
                    throw new IocContainerDependencyResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.CircularDependencyValidator_CheckCircularDependency_max_resolve_depth, MAX_RESOLVE_DEPTH));
                }

                if (m_TypeToValidateStack.Contains(typeToValidate))
                {
                    throw new IocContainerDependencyResolutionException(string.Format(CultureInfo.CurrentCulture, Strings.CircularDependencyValidator_CheckCircularDependency_Circular_dependency_detected, CreateDependencyGraphString(typeToValidate, m_TypeToValidateStack)));
                }

                m_TypeToValidateStack.Push(typeToValidate);
            }
        }

        /// <summary>
        /// Releases current thread entry that is used for circular dependency validation.
        /// </summary>
        public void Release()
        {
            lock (this)
            {
                m_TypeToValidateStack.Pop();
            }
        }

        /// <summary>
        ///  Finalizes an instance of the <see cref="CircularDependencyValidator"/> class.
        ///  Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~CircularDependencyValidator()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates the dependency graph string.
        /// </summary>
        /// <param name="typeToValidate">The type automatic validate.</param>
        /// <param name="typeToValidateStack">The type automatic validate stack.</param>
        /// <returns>The dependency graph string.</returns>
        private static string CreateDependencyGraphString(Type typeToValidate, IEnumerable<Type> typeToValidateStack)
        {
            StringBuilder dependencyGraphBuilder = new StringBuilder();
            foreach (Type type in typeToValidateStack.Reverse())
            {
                dependencyGraphBuilder.Append(type.FullName);
                dependencyGraphBuilder.Append(" -> ");
            }

            dependencyGraphBuilder.Append(typeToValidate);

            return dependencyGraphBuilder.ToString();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        private void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                m_TypeToValidateStack.Clear();
                m_TypeToValidateStack = null;

                m_Disposed = true;
            }
        }      
    }
}