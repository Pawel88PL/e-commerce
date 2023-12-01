using System.ComponentModel.DataAnnotations;

namespace MiodOdStaniula.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz swoje imię")]
        [StringLength(30, ErrorMessage = "Imię może zawierać maksymalnie 30 znaków.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz swoje nazwisko")]
        [StringLength(30, ErrorMessage = "Nazwisko może zawierać maksymalnie 30 znaków.")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
