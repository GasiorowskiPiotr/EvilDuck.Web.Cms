using System.Security.Claims;
using System.Threading.Tasks;
using EvilDuck.Platform.Entities;
using Microsoft.AspNet.Identity;

namespace EvilDuck.Platform.Core.Security
{
    public static class ApplicationUserExtensions
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(this ApplicationUser user, UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("EvilDuck.Id", user.Id));
            return userIdentity;
        } 
    }
}