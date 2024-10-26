using System.ComponentModel.DataAnnotations;

namespace Dashboard_Backend.Dtos
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El nombre completo es requerido")]
        [StringLength(80, ErrorMessage = "El nombre no puede exceder los 80 caracteres")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Debe ser una dirección de correo electrónico válida")]
        public string Email { get; set; }
    }
}
