using EvilDuck.Platform.Entities;
using Microsoft.AspNet.Identity;

namespace EvilDuck.Platform.Core.Security
{
    public class UsersManager : UserManager<ApplicationUser>
    {
        public UsersManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }
    }
}