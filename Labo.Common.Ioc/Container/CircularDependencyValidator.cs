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
    using System.Threading;

    using Labo.Common.Ioc.Container.Exceptions;
    using Labo.Common.Ioc.Exceptions;

    /// <summary>
    /// Circular dependency validator class.
    /// </summary>
    internal sealed class CircularDependencyValidator
    {
        /// <summary>
        /// The type to validate circular dependency.
        /// </summary>
        private readonly Type m_TypeToValidate;

        /// <summary>
        /// The threads list.
        /// </summary>
        private readonly HashSet<Thread> m_Threads = new HashSet<Thread>();

        /// <summary>
        /// The disabled
        /// </summary>
        private bool m_Disabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDependencyValidator"/> class.
        /// </summary>
        /// <param name="typeToValidate">The type to validate.</param>
        public CircularDependencyValidator(Type typeToValidate)
        {
            m_TypeToValidate = typeToValidate;
        }

        /// <summary>
        /// Disables validator.
        /// </summary>
        public void Disable()
        {
            m_Disabled = true;
        }

        /// <summary>
        /// Checks the circular dependency.
        /// </summary>
        /// <exception cref="IocContainerDependencyResolutionException">thrown when circular dependency detected.</exception>
        public void CheckCircularDependency()
        {
            if (m_Disabled)
            {
                return;
            }

            lock (this)
            {
                if (m_Threads.Contains(Thread.CurrentThread))
                {
                    throw new IocContainerDependencyResolutionException(string.Format(CultureInfo.CurrentCulture, "Circular dependency detected for the type '{0}'", m_TypeToValidate.FullName));
                }

                m_Threads.Add(Thread.CurrentThread);
            }
        }

        /// <summary>
        /// Releases current thread entry that is used for circular dependency validation.
        /// </summary>
        public void Release()
        {
            if (m_Disabled)
            {
                return;
            }

            lock (this)
            {
                m_Threads.Remove(Thread.CurrentThread);
            }
        }
    }
}