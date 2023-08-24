using System.ComponentModel.DataAnnotations;

namespace MiodOdStaniula.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Podaj nazwę użytkownika")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Podaj hasło")]
        public string? Password { get; set; }
    }
}
