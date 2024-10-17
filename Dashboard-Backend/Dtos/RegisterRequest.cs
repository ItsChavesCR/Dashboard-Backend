using System.ComponentModel.DataAnnotations;

namespace Dashboard_Backend.Dtos
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }
    }
}
