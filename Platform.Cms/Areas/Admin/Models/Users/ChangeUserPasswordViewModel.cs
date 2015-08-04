using System.ComponentModel.DataAnnotations;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Users
{
    public class ChangeUserPasswordViewModel
    {
        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Haslo jest wymagane")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Has³o jest za d³ugie")]
        [Display(Name = "Nowe has³o")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszê potwierdziæ has³o")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Has³o jest za d³ugie")]
        [Display(Name = "Powtórz nowe has³o")]
        public string ConfirmPassword { get; set; }
    }
}