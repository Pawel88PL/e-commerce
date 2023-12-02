using System.ComponentModel.DataAnnotations;

namespace MiodOdStaniula.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Adres email jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz swoje imię")]
        [StringLength(50, ErrorMessage = "Imię może zawierać maksymalnie 30 znaków.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz swoje nazwisko")]
        [StringLength(50, ErrorMessage = "Nazwisko może zawierać maksymalnie 30 znaków.")]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz miejscowość")]
        [StringLength(50, ErrorMessage = "Nazwa miejscowości jest za długa.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz ulicę")]
        [StringLength(50, ErrorMessage = "Nazwa ulicy jest za długa.")]
        public string Street { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz nr domu")]
        [StringLength(50, ErrorMessage = "Numer domu jest za długi.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Podaj kod pocztowy")]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Niepoprawny format kodu pocztowego.")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz nr telefonu")]
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "Hasło musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
