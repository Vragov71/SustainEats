using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using SustainEats.Shared.Models;
using SustainEats.Shared.Services;
using System.Threading.Tasks;

namespace SustainEats.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DatabaseService _dbService;

        public AuthController(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            using var connection = _dbService.GetConnection();
            await connection.OpenAsync();

            // Check if user exists
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(1) FROM Users WHERE Email = $email";
            checkCmd.Parameters.AddWithValue("$email", model.Email);
            var userExists = (long)await checkCmd.ExecuteScalarAsync() > 0;

            if (userExists)
            {
                return BadRequest("User with this email already exists.");
            }

            // Insert new user
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = "INSERT INTO Users (Username, Email, PasswordHash) VALUES ($username, $email, $passwordHash)";
            insertCmd.Parameters.AddWithValue("$username", model.Username);
            insertCmd.Parameters.AddWithValue("$email", model.Email);
            insertCmd.Parameters.AddWithValue("$passwordHash", BCrypt.Net.BCrypt.HashPassword(model.Password));
            
            await insertCmd.ExecuteNonQueryAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            using var connection = _dbService.GetConnection();
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT PasswordHash, Username FROM Users WHERE Email = $email";
            command.Parameters.AddWithValue("$email", model.Email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var storedHash = reader.GetString(0);
                var username = reader.GetString(1);

                if (BCrypt.Net.BCrypt.Verify(model.Password, storedHash))
                {
                    return Ok(new { Username = username });
                }
            }

            return Unauthorized();
        }
    }
}
