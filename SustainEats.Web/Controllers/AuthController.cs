using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SustainEats.Shared;
using SustainEats.Shared.Models;

namespace SustainEats.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return Unauthorized();
            }

            return Ok(new { Username = user.Username });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // In a real app with token-based auth, you'd invalidate the token here
            await Task.Delay(100);
            return Ok();
        }
    }
}