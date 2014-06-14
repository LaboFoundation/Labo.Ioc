namespace Labo.Common.Ioc.DryIoc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using global::DryIoc;

    /// <summary>
    /// Dry inversion of control container class.
    /// </summary>
    public sealed class DryIocContainer : BaseIocContainer
    {
        private readonly Container m_Container;

        public DryIocContainer()
        {
            m_Container = new Container();
        }

        public override void RegisterSingleInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            m_Container.RegisterDelegate(x => creator(this), Reuse.Singleton);
        }

        public override void RegisterSingleInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            m_Container.RegisterDelegate(x => creator(this), Reuse.Singleton, null, name);
        }

        public override void RegisterSingleInstance(Type serviceType)
        {
            m_Container.Register(serviceType, Reuse.Singleton);
        }

        public override void RegisterInstance(Type serviceType)
        {
            m_Container.Register(serviceType, Reuse.Transient);
        }

        public override void RegisterSingleInstance(Type serviceType, Type implementationType)
        {
            m_Container.Register(serviceType, implementationType, Reuse.Singleton);
        }

        public override void RegisterSingleInstanceNamed(Type serviceType, Type implementationType, string name)
        {
            m_Container.Register(serviceType, implementationType, Reuse.Singleton, null, null, name);
        }

        public override void RegisterSingleInstanceNamed(Type serviceType, string name)
        {
            m_Container.Register(serviceType, Reuse.Singleton, null, null, name);
        }

        public override void RegisterInstance<TImplementation>(Func<IIocContainerResolver, TImplementation> creator)
        {
            m_Container.RegisterDelegate(x => creator(this), Reuse.Transient);
        }

        public override void RegisterInstance(Type serviceType, Type implementationType)
        {
            m_Container.Register(serviceType, implementationType, Reuse.Transient);
        }

        public override void RegisterInstanceNamed<TImplementation>(Func<IIocContainerResolver, TImplementation> creator, string name)
        {
            m_Container.RegisterDelegate(x => creator(this), Reuse.Transient, null, name);
        }

        public override void RegisterInstanceNamed(Type serviceType, Type implementationType, string name)
        {
            m_Container.Register(serviceType, implementationType, Reuse.Transient, null, null, name);
        }

        public override void RegisterInstanceNamed(Type serviceType, string name)
        {
            m_Container.Register(serviceType, Reuse.Transient, null, null, name);
        }

        public override object GetInstance(Type serviceType, object[] parameters)
        {
            return m_Container.Resolve(serviceType);
        }

        public override object GetInstance(Type serviceType)
        {
            return m_Container.Resolve(serviceType);
        }

        public override object GetInstanceByName(Type serviceType, string name, object[] parameters)
        {
            return m_Container.Resolve(serviceType, name);
        }

        public override object GetInstanceByName(Type serviceType, string name)
        {
            return m_Container.Resolve(serviceType, name);
        }

        public override object GetInstanceOptional(Type serviceType, object[] parameters)
        {
            return m_Container.IsRegistered(serviceType) ? m_Container.Resolve(serviceType) : null;
        }

        public override object GetInstanceOptional(Type serviceType)
        {
            return m_Container.IsRegistered(serviceType) ? m_Container.Resolve(serviceType) : null;
        }

        public override object GetInstanceOptionalByName(Type serviceType, string name, object[] parameters)
        {
            return m_Container.IsRegistered(serviceType, name) ? m_Container.Resolve(serviceType, name) : null;
        }

        public override object GetInstanceOptionalByName(Type serviceType, string name)
        {
            return m_Container.IsRegistered(serviceType, name) ? m_Container.Resolve(serviceType, name) : null;
        }

        public override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            Type serviceTypeEnumerable = typeof(IEnumerable<>).MakeGenericType(new[] { serviceType });

            object obj = m_Container.Resolve(serviceTypeEnumerable);
            return ((IEnumerable)obj).Cast<object>();
        }

        public override bool IsRegistered(Type type)
        {
            return m_Container.IsRegistered(type);
        }

        public override bool IsRegistered(Type type, string name)
        {
            return m_Container.IsRegistered(type, name);
        }
    }
}
