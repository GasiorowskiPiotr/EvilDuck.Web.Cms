using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Users
{
    public class AssignRolesViewModel
    {
        public string UserId { get; set; }

        public IList<string> AllRoles { get; set; }

        [Display(Name = "Wybrane role")]
        public IList<string> SelectedRoles { get; set; }
    }
}