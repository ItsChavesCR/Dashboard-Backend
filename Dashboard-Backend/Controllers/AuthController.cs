using Dashboard_Backend.Dtos;
using Dashboard_Backend.Models;
using Dashboard_Backend.Services;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;

namespace Dashboard_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;

        // Simulación de una base de datos en memoria
        private static List<User> Users = new List<User>();

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Simular validación de usuario (esto debería hacerse con una base de datos real)
            var user = Users.FirstOrDefault(u => u.Username == loginRequest.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                return Unauthorized("Nombre de usuario o contraseña incorrectos");
            }

            // Generar el token JWT
            var token = _tokenService.GenerateToken(user.UserId.ToString());

            return Ok(new { Token = token });
        }

        // Endpoint de registro de usuarios
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar si el usuario ya existe
                if (Users.Any(u => u.Username == registerRequest.Username))
                {
                    return BadRequest("El nombre de usuario ya está en uso.");
                }

                //// Hashear la contraseña
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

                // Crear nuevo usuario y agregarlo a la lista simulada
                var newUser = new User
                {
                    UserId = Users.Count + 1,  // Simulación de incremento de ID
                    Username = registerRequest.Username,
                    PasswordHash = passwordHash
                };

                Users.Add(newUser);

                return Ok("Usuario registrado exitosamente.");
            }
            catch (Exception ex)
            {
                // Registrar el error y devolver una respuesta de error genérica
                Console.WriteLine($"Error en el registro: {ex.Message}");
                return StatusCode(500, "Ocurrió un error en el servidor.");
            }
        }

    }
}
