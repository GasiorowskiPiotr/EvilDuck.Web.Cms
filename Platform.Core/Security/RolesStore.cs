using Microsoft.AspNet.Identity.EntityFramework;

namespace EvilDuck.Platform.Core.Security
{
    public class RolesStore : RoleStore<IdentityRole>
    {
        public RolesStore(ApplicationDbContext a) : base(a)
        {
            
        }
    }
}