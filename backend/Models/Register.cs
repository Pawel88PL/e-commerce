using System.ComponentModel.DataAnnotations;

namespace MiodOdStaniula.Models
{
    public class Register
    {
        
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
