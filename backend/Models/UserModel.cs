using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MiodOdStaniula.Models
{
    public class UserModel: IdentityUser
    {
        [Required(ErrorMessage = "Wpisz swoje imię")]
        [StringLength(50, ErrorMessage = "Imię jest za długie.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wpisz swoje nazwisko")]
        [StringLength(50, ErrorMessage = "Nazwisko jest za długie.")]
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
    }

    public class UserDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
