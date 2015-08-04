using System.ComponentModel.DataAnnotations;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Users
{
    public class ChangeUserPasswordViewModel
    {
        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Haslo jest wymagane")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Has�o jest za d�ugie")]
        [Display(Name = "Nowe has�o")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Prosz� potwierdzi� has�o")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Has�o jest za d�ugie")]
        [Display(Name = "Powt�rz nowe has�o")]
        public string ConfirmPassword { get; set; }
    }
}