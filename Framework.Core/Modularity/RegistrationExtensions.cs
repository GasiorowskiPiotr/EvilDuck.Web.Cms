using Autofac;
using EvilDuck.Framework.Core.Cache;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.Modularity
{
    public static class RegistrationExtensions
    {
        public static void RegisterDomainContext<TDomainContext>(this ContainerBuilder cb) where TDomainContext : DomainContext
        {
            cb.RegisterType<TDomainContext>().AsSelf().InstancePerRequest();
        }

        public static void RegisterDomainContext<TDomainContext, TInterface>(this ContainerBuilder cb)
            where TDomainContext : DomainContext, TInterface
        {
            cb.RegisterType<TDomainContext>().AsSelf().As<TInterface>().InstancePerRequest();
        }

        public static void RegisterEntityRepository<TEntityRepository, TDomainContext, TEntity, TKey>(
            this ContainerBuilder cb)
            where TEntityRepository : EntityRepository<TDomainContext, TEntity, TKey>
            where TDomainContext : DomainContext
            where TEntity : Entity<TKey>
        {
            cb.RegisterType<TEntityRepository>().AsSelf().InstancePerRequest();
        }

        public static void RegisterEntityRepository<TEntityRepository, TInterface, TDomainContext, TEntity, TKey>(
            this ContainerBuilder cb)
            where TEntityRepository : EntityRepository<TDomainContext, TEntity, TKey>, TInterface
            where TDomainContext : DomainContext
            where TEntity : Entity<TKey>
        {
            cb.RegisterType<TEntityRepository>().As<TInterface>().InstancePerRequest();
        }

        public static void RegisterUnitOfWork<TUnitOfWork, TDomainContext>(this ContainerBuilder cb)
            where TUnitOfWork : IUnitOfWork
            where TDomainContext : DomainContext
        {
            cb.RegisterType<TUnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
        }

        public static void RegisterUnitOfWork<TUnitOfWork, TIUnitOfWork, TDomainContext>(this ContainerBuilder cb)
            where TUnitOfWork : UnitOfWork<TDomainContext>, TIUnitOfWork
            where TDomainContext : DomainContext
            where TIUnitOfWork : IUnitOfWork
        {
            cb.RegisterType<TUnitOfWork>().As<TIUnitOfWork>().InstancePerRequest();
        }

        public static void RegisterCache<TCache>(this ContainerBuilder cb) where TCache : CustomCache
        {
            cb.RegisterType<TCache>().AsSelf().SingleInstance();
        }

        public static void RegisterCache<TCache, TICache>(this ContainerBuilder cb) where TCache : CustomCache, TICache
        {
            cb.RegisterType<TCache>().As<TICache>().SingleInstance();
        }
    }
}