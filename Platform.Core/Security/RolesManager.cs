using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EvilDuck.Platform.Core.Security
{
    public class RolesManager : RoleManager<IdentityRole>
    {
        public RolesManager(IRoleStore<IdentityRole, string> store) : base(store)
        {
        }
    }
}