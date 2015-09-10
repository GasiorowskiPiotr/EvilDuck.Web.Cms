using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using EvilDuck.Platform.Cms.Models;
using EvilDuck.Platform.Core.DataFramework.Logic;
using EvilDuck.Platform.Core.DataFramework.Repositories;
using EvilDuck.Platform.Entities.DataFramework;

namespace EvilDuck.Platform.Cms.Controllers
{
    [RoutePrefix("api/Data/Queries")]
    public class QueryController : ApiController
    {
        private readonly QueriesRepository _queriesRepository;
        private readonly QueryComponentFactory _queryComponentFactory;

        public QueryController(QueriesRepository queriesRepository, QueryComponentFactory queryComponentFactory)
        {
            _queriesRepository = queriesRepository;
            _queryComponentFactory = queryComponentFactory;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var queries = await _queriesRepository.AdHocQuery().Select(e => new QueryDto
            {
                Description = e.Caption,
                Name = e.Name,
                QueryType = e.Type
            }).ToListAsync();

            return Ok(queries);
        }

        [HttpGet]
        [Route("{method}")]
        public async Task<IHttpActionResult> Get(string method, IDictionary<string, object> parameters)
        {
            var query = await _queriesRepository.GetByNameAndTypes(method, QueryType.Select);

            var queryComponent = _queryComponentFactory.CreateQueryComponent(query.Id);

            QueryResult queryResult;
            var result = queryComponent.ExecuteQuery(out queryResult, parameters);

            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("{method}")]
        public async Task<IHttpActionResult> Post(string method, IDictionary<string, object> parameters)
        {
            var query = await _queriesRepository.GetByNameAndTypes(method, QueryType.Insert, QueryType.Delete, QueryType.Update);

            var queryComponent = _queryComponentFactory.CreateQueryComponent(query.Id);

            QueryResult queryResult;
            var result = queryComponent.ExecuteQuery(out queryResult, parameters);

            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

    }
}
