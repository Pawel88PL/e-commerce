using System.ComponentModel.DataAnnotations;

namespace MiodOdStaniula.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Required (ErrorMessage = "Wpisz swoje imię")]
        [StringLength(50, ErrorMessage = "Imię jest za długie.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Wpisz swoje nazwisko")]
        [StringLength(50, ErrorMessage = "Nazwisko jest za długie.")]
        public string? Surname { get; set; }

        [Required(ErrorMessage = "Wpisz nr telefonu")]
        [Phone(ErrorMessage = "Niepoprawny format numeru telefonu.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Wpisz swój adres email")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Wpisz miejscowość")]
        [StringLength(50, ErrorMessage = "Nazwa miejscowości jest za długa.")]
        public string? City { get; set; }

        [Required(ErrorMessage = "Wpisz ulicę")]
        [StringLength(50, ErrorMessage = "Nazwa ulicy jest za długa.")]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Wpisz nr domu")]
        [StringLength(20, ErrorMessage = "Numer domu jest za długi.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Podaj kod pocztowy")]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Niepoprawny format kodu pocztowego.")]
        public string? PostalCode { get; set; }

        public virtual ICollection<Order>? Orders { get; set; }

    }
}
