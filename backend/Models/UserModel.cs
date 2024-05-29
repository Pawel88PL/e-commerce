using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MiodOdStaniula.Models
{
    public class UserModel: IdentityUser
    {
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

        public DateOnly RegistrationDate { get; set; }
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
        public DateOnly RegistrationDate { get; set; }
        public bool EmailConfirmed { get; set; }
    }

    public class ChangePasswordModel
    {
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
