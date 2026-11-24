using Microsoft.AspNetCore.Mvc;
using CarRental.Api.Data;
using CarRental.Api.Models;

namespace CarRental.Controllers;

public record LoginRequest(string Email, string Password);

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = FakeStore.Users.FirstOrDefault(
            u => u.Email == request.Email && u.Password == request.Password);

        if (user is null)
            return Unauthorized(new { message = "Correo o contraseña incorrectos." });

        return Ok(new
        {
            message = "Inicio de sesión exitoso",
            user = new { user.Id, user.Username, user.Email }
        });
    }
}