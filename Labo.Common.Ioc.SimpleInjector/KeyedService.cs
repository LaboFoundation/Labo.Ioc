// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyedService.cs" company="Labo">
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
//   Keyed service class
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Labo.Common.Ioc.SimpleInjector
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Keyed service class
    /// </summary>
    internal sealed class KeyedService : IComparable
    {
        /// <summary>
        /// The service key
        /// </summary>
        private readonly string m_ServiceKey;

        /// <summary>
        /// The service type
        /// </summary>
        private readonly Type m_ServiceType;

        /// <summary>
        /// Gets the key of the service.
        /// </summary>
        /// <value>The key of the service.</value>
        public string ServiceKey
        {
            get
            {
                return m_ServiceKey;
            }
        }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public Type ServiceType
        {
            get
            {
                return m_ServiceType;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyedService"/> class.
        /// </summary>
        /// <param name="serviceKey">The service key.</param>
        /// <param name="serviceType">Type of the service.</param>
        public KeyedService(string serviceKey, Type serviceType)
        {
            if (serviceKey == null)
            {
                throw new ArgumentNullException("serviceKey");
            }

            if (serviceType == null)
            {
                throw new ArgumentNullException("serviceType");
            }

            m_ServiceKey = serviceKey;
            m_ServiceType = serviceType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            KeyedService keyedService = obj as KeyedService;
            return keyedService != null && ServiceKey.Equals(keyedService.ServiceKey) && ServiceType == keyedService.ServiceType;
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return ServiceKey.GetHashCode() ^ ServiceType.GetHashCode();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj"/> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj"/>. Greater than zero This instance follows <paramref name="obj"/> in the sort order. 
        /// </returns>
        /// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception><filterpriority>2</filterpriority>
        public int CompareTo(object obj)
        {
            KeyedService keyedService = (KeyedService)obj;
            int compare = Comparer<string>.Default.Compare(ServiceKey, keyedService.ServiceKey);
            if (compare == 0)
            {
                return ServiceType.GetHashCode() - keyedService.ServiceType.GetHashCode();
            }

            return compare;
        }
    }
}