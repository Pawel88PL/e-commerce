using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Register
    {
        [StringLength(50)]
        public string Email { get; set; } = string.Empty;

        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string Surname { get; set; } = string.Empty;

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? Street { get; set; }

        [StringLength(50)]
        public string? Address { get; set; }

        [StringLength(6)]
        public string? PostalCode { get; set; }

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        public bool IsGuestClient { get; set; }
        public bool TermsAccepted { get; set; }

        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
    }
}
