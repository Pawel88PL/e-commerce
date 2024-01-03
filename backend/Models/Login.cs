using System.ComponentModel.DataAnnotations;

namespace MiodOdStaniula.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
