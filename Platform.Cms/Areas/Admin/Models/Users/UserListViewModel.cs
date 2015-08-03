using EvilDuck.Platform.Entities;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Users
{
    public class UserListViewModel
    {
        public string Username { get; set; }
        public string Id { get; set; }

        public static UserListViewModel FromEntity(ApplicationUser user)
        {
            return new UserListViewModel
            {
                Id = user.Id,
                Username = user.Email,
            };
        }
    }
}