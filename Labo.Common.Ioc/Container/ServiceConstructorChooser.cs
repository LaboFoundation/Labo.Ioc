// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceConstructorChooser.cs" company="Labo">
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
//   Service constructor chooser class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Labo.Common.Ioc.Container.Exceptions;
    using Labo.Common.Ioc.Exceptions;
    using Labo.Common.Ioc.Resources;

    /// <summary>
    /// Service constructor chooser class.
    /// </summary>
    internal sealed class ServiceConstructorChooser : IServiceConstructorChooser
    {
        /// <summary>
        /// The constructor binding flags
        /// </summary>
        private const BindingFlags CONSTRUCTOR_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        /// <summary>
        /// Gets the constructor.
        /// </summary>
        /// <param name="serviceImplementationType">Type of the service implementation.</param>
        /// <returns>ConstructorInfo class.</returns>
        public ConstructorInfo GetConstructor(Type serviceImplementationType)
        {
            if (serviceImplementationType == null)
            {
                throw new ArgumentNullException("serviceImplementationType");
            }

            ConstructorInfo constructor = serviceImplementationType.GetConstructors(CONSTRUCTOR_BINDING_FLAGS).FirstOrDefault();

            if (constructor == null)
            {
                throw new IocContainerDependencyResolutionException(
                    string.Format(
                        CultureInfo.CurrentCulture,
                        Strings.LaboIocEmitServiceCreator_CreateServiceInstance_NoConstructorsCanBeFound,
                        serviceImplementationType.FullName));
            }

            return constructor;
        }
    }
}