using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EvilDuck.Framework.Core.Utils;
using EvilDuck.Platform.Cms.Areas.Admin.Models.Users;
using EvilDuck.Platform.Core.Security;
using EvilDuck.Platform.Entities;
using Microsoft.AspNet.Identity;
using NLog;

namespace EvilDuck.Platform.Cms.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly Logger _logger;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, Logger logger)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;

        }

        [Route("")]
        public ActionResult Index()
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info("Getting all the users from DB.");
            }

            var users = _context.Users.Select(UserListViewModel.FromEntity).ToList();
            return View(users);
        }

        [Route("[action]")]
        public ActionResult Add()
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info("Generating view for adding users.");
            }

            return View(new AddUserViewModel());
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Add(AddUserViewModel vm)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info("Starting adding new user.");
            }

            if (!ModelState.IsValid)
            {
                if (_logger.IsWarnEnabled)
                {
                    _logger.Warn(String.Format("Not all the fileds are valid. Model state: {0}", String.Join(";", ModelState.Select(e => String.Format("{0} = {1}", e.Key, String.Join(",", e.Value.Errors.Select(e2 => e2.ErrorMessage)))))));
                }

                return View(vm);
            }

            var result = await _userManager.CreateAsync(new ApplicationUser
            {
                Email = vm.Email,
                UserName = vm.Email,
            }, vm.Password);

            if (result.Succeeded)
            {
                if (_logger.IsInfoEnabled)
                {
                    _logger.Info(String.Format("User: {0} created.", vm.Email));
                }

                return RedirectToAction("Index");
            }
            result.Errors.Do(e => ModelState.AddModelError(string.Empty, e));
            if (_logger.IsWarnEnabled)
            {
                _logger.Warn(String.Format("Could not create user. Model state: {0}", String.Join(";", ModelState.Select(e => String.Format("{0} = {1}", e.Key, String.Join(",", e.Value.Errors.Select(e2 => e2.ErrorMessage)))))));
            }

            return View(vm);
        }

        [Route("[action]")]
        public async Task<ActionResult> Remove(string id)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info(String.Format("Removing user with ID: {0}.", id));
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [Route("[action]")]
        public ActionResult ChangePassword(string id)
        {
            return View(new ChangeUserPasswordViewModel
            {
                UserId = id
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> ChangePassword(ChangeUserPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var user = await _userManager.FindByIdAsync(vm.UserId);
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Użytkownik nie istnieje w bazie danych.");
                return View(vm);
            }

            var result = await _userManager.RemovePasswordAsync(user.Id);
            if (!result.Succeeded)
            {
                result.Errors.Do(e => ModelState.AddModelError(String.Empty, new Exception(e)));
            }
            result = await _userManager.AddPasswordAsync(user.Id, vm.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            result.Errors.Do(e => ModelState.AddModelError(String.Empty, new Exception(e)));
            return View(vm);
        }

        public async Task<ActionResult> AssignRoles(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new AssignRolesViewModel
            {
                UserId = user.Id,
                AllRoles = await _context.Roles.Select(r => r.Name).ToListAsync(),
                SelectedRoles = user.Roles.Join(_context.Roles, e=> e.RoleId, e => e.Id, (iur ,ir) => ir.Name).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> AssignRoles(AssignRolesViewModel vm)
        {
            vm.AllRoles = await _context.Roles.Select(r => r.Name).ToListAsync();
            var rolesToAdd = new List<string>();
            var rolesToRemove = new List<string>();
            foreach (var role in vm.AllRoles)
            {
                if (vm.SelectedRoles.Contains(role) && !await _userManager.IsInRoleAsync(vm.UserId, role))
                {
                    rolesToAdd.Add(role);
                }
                else if (!vm.SelectedRoles.Contains(role) && await _userManager.IsInRoleAsync(vm.UserId, role))
                {
                    rolesToRemove.Add(role);
                }
            }

            var result = await _userManager.AddToRolesAsync(vm.UserId, rolesToAdd.ToArray());
            if (!result.Succeeded)
            {
                result.Errors.Do(e => ModelState.AddModelError(String.Empty, e));
            }
            result = await _userManager.RemoveFromRolesAsync(vm.UserId, rolesToRemove.ToArray());
            if (!result.Succeeded)
            {
                result.Errors.Do(e => ModelState.AddModelError(String.Empty, e));
            }

            return View(vm);
        }
    }
}