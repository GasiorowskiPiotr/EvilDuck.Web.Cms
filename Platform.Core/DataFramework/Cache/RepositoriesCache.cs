using System.Globalization;
using EvilDuck.Framework.Core.Cache;
using EvilDuck.Platform.Core.DataAccess;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;
using NLog;

namespace EvilDuck.Platform.Core.DataFramework.Cache
{
    public class RepositoriesCache : CustomCache
    {
        public RepositoriesCache(Logger logger) : base(logger)
        {
        }

        public Repository Get(string name)
        {
            return (Repository) Get(new RepositoryByNameCacheKey(name));
        }

        public Repository Get(int id)
        {
            return (Repository) Get(new RepositoryByIdCacheKey(id));
        }

        protected override void OnMiss(ICacheKey key, out object value)
        {
            var repository = new RepositoriesRepository(new PlatformDomainContext(), Logger);

            var keyById = key as RepositoryByIdCacheKey;
            if (keyById != null)
            {
                var repo = repository.GetByKey(keyById.Id);
                if (repo != null)
                {
                    Add(key, repo);
                    Add(new RepositoryByNameCacheKey(repo.Name), repo);
                    value = repo;
                    return;
                }
                
            }

            var keyByName = key as RepositoryByNameCacheKey;
            if (keyByName != null)
            {

                var repo = repository.GetByName(keyByName.Name);
                if (repo != null)
                {
                    Add(key, repo);
                    Add(new RepositoryByIdCacheKey(repo.Id), repo);
                    value = repo;
                    return;
                }
            }

            value = null;
        }

        class RepositoryByIdCacheKey : ICacheKey
        {
            public int Id { get; private set; }

            public RepositoryByIdCacheKey(int id)
            {
                Id = id;
            }

            public string KeyAsString
            {
                get { return Id.ToString(CultureInfo.InvariantCulture); }
            }
        }

        class RepositoryByNameCacheKey : ICacheKey
        {
            public RepositoryByNameCacheKey(string name)
            {
                Name = name;
            }

            public string Name { get; private set; }

            public string KeyAsString
            {
                get { return Name; }
            }
        }
    }
}