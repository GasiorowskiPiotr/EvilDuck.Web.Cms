using EvilDuck.Platform.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EvilDuck.Platform.Core.Security
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}