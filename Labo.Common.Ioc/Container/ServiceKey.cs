// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceKey.cs" company="Labo">
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
//   Service key.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.Container
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Service key.
    /// </summary>
    internal struct ServiceKey : IEquatable<ServiceKey>
    {
        /// <summary>
        /// The m_ hash code
        /// </summary>
        private readonly int m_HashCode;

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceKey"/> struct.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="serviceType">Type of the service.</param>
        public ServiceKey(string serviceName, Type serviceType)
             : this()
        {
            Debug.Assert(serviceType == null, "serviceType cannot be null");

            ServiceName = serviceName;
            ServiceType = serviceType;
            m_HashCode = GetHashCodeInternal();
        }

        /// <summary>
        ///  Determines whether two specified instances of <see cref="T:Labo.Common.Ioc.Container.ServiceKey"/> are equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns> true if <paramref name="left"/> and <paramref name="right"/> represent the same ServiceKey; otherwise, false.</returns>
        public static bool operator ==(ServiceKey left, ServiceKey right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two specified instances of <see cref="T:Labo.Common.Ioc.Container.ServiceKey"/> are not equal.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns> true if <paramref name="left"/> and <paramref name="right"/> do not represent the same ServiceKey; otherwise, false.</returns>
        public static bool operator !=(ServiceKey left, ServiceKey right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(ServiceKey other)
        {
            return ServiceType == other.ServiceType && string.Equals(ServiceName, other.ServiceName, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return m_HashCode;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        /// <param name="obj">Another object to compare to. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is ServiceKey && Equals((ServiceKey)obj);
        }

        /// <summary>
        /// Gets the hash code internal.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        private int GetHashCodeInternal()
        {
            unchecked
            {
                int hash = ServiceType.GetHashCode();
                if (ServiceName != null)
                {
                    hash ^= ServiceName.GetHashCode();
                }

                return hash;
            }
        }
    }
}