using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EvilDuck.Framework.Core.Utils;
using EvilDuck.Platform.Cms.Areas.Admin.Models.Roles;
using EvilDuck.Platform.Cms.Models;
using EvilDuck.Platform.Core.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;

namespace EvilDuck.Platform.Cms.Areas.Admin.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly RoleManager<IdentityRole> _rolesManager;

        private readonly Logger _logger;

        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context, Logger logger)
        {
            _rolesManager = roleManager;
            _context = context;
            _logger = logger;
        }

        [Route("")]
        public ActionResult Index()
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info("Getting roles from Database.");
            }

            var roles = _context.Roles.Select(RoleListViewModel.FromEntity).ToList();
            return View(roles);
        }

        [Route("[action]")]
        public ActionResult Add()
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info("Generating view for adding roles.");
            }

            return View(new AddRoleViewModel());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Add(AddRoleViewModel vm)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(String.Format("Starting adding new role: {0}", vm.Name));
            }

            if (!ModelState.IsValid)
            {
                if (_logger.IsWarnEnabled)
                {
                    _logger.Warn(String.Format("Not all the fileds are valid. Model state: {0}", String.Join(";", ModelState.Select(e => String.Format("{0} = {1}", e.Key, String.Join(",", e.Value.Errors.Select(e2 => e2.ErrorMessage)))))));
                }

                return View(vm);
            }

            var result = await _rolesManager.CreateAsync(new IdentityRole
            {
                Name = vm.Name
            });
            if (result.Succeeded)
            {
                if (_logger.IsInfoEnabled)
                {
                    _logger.Info(String.Format("Role: {0} created.", vm.Name));
                }

                return RedirectToAction("Index");
            }
            result.Errors.Do(e => ModelState.AddModelError(string.Empty, e));
            if (_logger.IsWarnEnabled)
            {
                _logger.Warn(String.Format("Could not create role. Model state: {0}", String.Join(";", ModelState.Select(e => String.Format("{0} = {1}", e.Key, String.Join(",", e.Value.Errors.Select(e2 => e2.ErrorMessage)))))));
            }

            return View(vm);
        }

        [Route("[action]")]
        public async Task<ActionResult> Remove(string id)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(String.Format("Removing role with ID: {0}.", id));
            }

            var role = await _context.Roles.SingleOrDefaultAsync(r => r.Id == id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}