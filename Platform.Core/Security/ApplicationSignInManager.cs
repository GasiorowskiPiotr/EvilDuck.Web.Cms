using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using EvilDuck.Platform.Entities;
using Microsoft.AspNet.Identity.Owin;

namespace EvilDuck.Platform.Core.Security
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, HttpContextBase context) :
            base(userManager, context.GetOwinContext().Authentication)
        {
            
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
    }
}