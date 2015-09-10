using System;
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
    [RoutePrefix("api/Data/Repositories")]
    public class RepositoryController : ApiController
    {
        private readonly RepositoriesRepository _repositoriesRepository;
        private readonly QueryComponentFactory _queryComponentFactory;

        public RepositoryController(RepositoriesRepository repositoriesRepository, QueryComponentFactory queryComponentFactory)
        {
            _repositoriesRepository = repositoriesRepository;
            _queryComponentFactory = queryComponentFactory;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<RepositoryDto>> Get()
        {
            return await _repositoriesRepository.AdHocQuery().Select(e => new RepositoryDto()
            {
                Description = e.Caption,
                Name = e.Name,
                CanDelete = e.DeleteQuery != null,
                CanInsert = e.InsertQuery != null,
                CanUpdate = e.UpdateQuery != null,
                SelectQueries = e.SelectQueries.Select(q => q.Name).ToList()
            }).ToListAsync();
        }

        [HttpGet]
        [Route("{id}/select/{method}")]
        public async Task<IHttpActionResult> Get(string id, string method, IDictionary<string, object> parameters)
        {
            var repository = await _repositoriesRepository.GetByNameAsync(id);
            if (repository == null)
            {
                return NotFound();
            }

            Query query = repository.SelectQueries.SingleOrDefault(e => e.Name == method);
            
            if (query == null)
            {
                return NotFound();
            }

            QueryResult queryResult;
            var queryComponent = _queryComponentFactory.CreateQueryComponent(query.Id);

            var result = queryComponent.ExecuteQuery(out queryResult, parameters);
            if (result.IsSuccess)
            {
                return Ok(queryResult);
            }

            return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("{id}/{method}")]
        public async Task<IHttpActionResult> Post(string id, string method, IDictionary<string, object> parameters)
        {
            var repository = await _repositoriesRepository.GetByNameAsync(id);
            if (repository == null)
            {
                return NotFound();
            }

            Query query;
            if (String.Equals(method, "INSERT", StringComparison.InvariantCultureIgnoreCase))
            {
                query = repository.InsertQuery;
            }
            else if (String.Equals(method, "UPDATE", StringComparison.InvariantCultureIgnoreCase))
            {
                query = repository.UpdateQuery;
            }
            else if (String.Equals(method, "DELETE", StringComparison.InvariantCultureIgnoreCase))
            {
                query = repository.DeleteQuery;
            }
            else
            {
                return BadRequest("Unknown method");
            }

            var queryComponent = _queryComponentFactory.CreateQueryComponent(query.Id);
            var result = queryComponent.ExecuteAsNonQuery(parameters);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }
    }
}