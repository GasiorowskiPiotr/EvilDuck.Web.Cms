using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    [Authorize]
    public class SystemParametersController :
        ApiCrudController<SystemParametersDomainContext, SystemParametersRepository, SystemParameter, string>
    {
        public SystemParametersController(SystemParametersRepository repository, IUnitOfWork<SystemParametersDomainContext> unitOfWork) : base(repository, unitOfWork)
        {
        }

        [HttpGet, Route("")]
        public async Task<IHttpActionResult> List(QueryModel queryModel)
        {
            var results = await GetItems(queryModel);
            return
                Ok(
                    new ListResult<SystemParametersListViewModel>(
                        results.Entities.Select(e => new SystemParametersListViewModel(e)), results.AllCount,
                        results.QueryModel));

        }

        [HttpGet, Route("{id}", Name = "GetSystemParameterById")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var result = await GetItemAsync(id);
            return Ok(result);
        }

        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Post(SystemParameterEditorViewModel vm)
        {
            var param = await CreateFromAsync(vm);
            return CreatedAtRoute("GetSystemParameterById", new {id = param.Id}, param);
        }

        [HttpDelete, Route("{id}")]
        public async Task<IHttpActionResult> Delete(string id)
        {
            await this.RemoveAsync(id);
            return Ok();
        }
    }
}
