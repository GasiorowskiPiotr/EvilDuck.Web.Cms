using EvilDuck.Platform.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EvilDuck.Platform.Core.Security
{
    public class UsersStore : UserStore<ApplicationUser>
    {
        public UsersStore(ApplicationDbContext a) : base(a)
        {
            
        }
    }
}