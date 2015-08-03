using Microsoft.AspNet.Identity.EntityFramework;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Roles
{
    public class RoleListViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public static RoleListViewModel FromEntity(IdentityRole role)
        {
            return new RoleListViewModel
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }
}