using System.Collections.Generic;
using System.Linq;
using EvilDuck.Framework.Entities;

namespace EvilDuck.Framework.Core.Web
{
    public class ListResult<TEntity> where TEntity: class
    {
        public IEnumerable<TEntity> Entities { get; private set; }
        public int AllCount { get; private set; }
        public QueryModel QueryModel { get; private set; }

        public ListResult(IEnumerable<TEntity> entities, int allCount, QueryModel queryModel)
        {
            Entities = entities;
            AllCount = allCount;
            QueryModel = queryModel;
        }

        public ListResult<T> Map<TEntity2, T>() where T : class, IEntityListViewModel<TEntity2>, new() where TEntity2 : Entity
        {
            return new ListResult<T>(this.Entities.Select(e =>
            {
                var vm = new T();
                vm.FillFromEntity(e as TEntity2);

                return vm;
            }),
            AllCount = AllCount,
            QueryModel = QueryModel);
        } 
    }
}