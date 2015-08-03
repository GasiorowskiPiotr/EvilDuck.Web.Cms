using System.Collections.Generic;

namespace EvilDuck.Framework.Core.Web
{
    public class ListResult<TEntity> where TEntity : class
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
    }
}