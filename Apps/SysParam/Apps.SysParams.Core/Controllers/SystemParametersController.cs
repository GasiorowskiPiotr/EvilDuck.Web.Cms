using System.Threading.Tasks;
using System.Web.Http;
using EvilDuck.Applications.SystemParameters.Core.DataAccess;
using EvilDuck.Applications.SystemParameters.Core.Models;
using EvilDuck.Applications.SystemParameters.Entities;
using EvilDuck.Framework.Core.DataAccess;
using EvilDuck.Framework.Core.Web;
using EvilDuck.Framework.Core.Web.Api;

namespace EvilDuck.Applications.SystemParameters.Core.Controllers
{
    [RoutePrefix("api/SystemParameters")]
    public class SystemParametersController :
        ApiCrudController<SystemParametersDomainContext, SystemParametersRepository, SystemParameter, string>
    {
        public SystemParametersController(SystemParametersRepository repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> List(QueryModel queryModel)
        {
            var results = await GetItems(queryModel);
            return Ok(results.Map<SystemParameter, SystemParametersListViewModel>());
        }

        [HttpGet, Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var result = await GetItemAsync(id);
            return Ok(result);
        }


    }
}
