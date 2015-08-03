using System.ComponentModel.DataAnnotations;

namespace EvilDuck.Platform.Cms.Areas.Admin.Models.Users
{
    public class AddUserViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email jest wymagany")]
        [EmailAddress]
        [MaxLength(64, ErrorMessage = "Email jest za długi")]
        [Display(Name = "Adres email")]
        public string Email { get; set; }

        [Display(Name = "Hasło")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Haslo jest wymagane")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Hasło jest za długie")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Proszę potwierdzić hasło")]
        [Compare("Password", ErrorMessage = "Hasło i potwierdzenie nie są zgodne.")]
        [DataType(DataType.Password)]
        [MaxLength(64, ErrorMessage = "Hasło jest za długie")]
        [Display(Name = "Powtórz hasło")]
        public string ConfirmPassword { get; set; }
    }
}