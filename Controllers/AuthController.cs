using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using ToDoList_RestAPI.Data;
using ToDoList_RestAPI.Models;
using ToDoList_RestAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ToDoList_RestAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserInsert userInsert)
        {
            try
            {
                if (_context.Users.Any(u => u.Username == userInsert.Username || u.Email == userInsert.Email))
                {
                    return BadRequest(Messages.User.UserExists);
                }

                userInsert.Password = BCrypt.Net.BCrypt.HashPassword(userInsert.Password);

                var user = new User
                {
                    Username = userInsert.Username,
                    FirstName = userInsert.FirstName,
                    LastName = userInsert.LastName,
                    BirthDate = userInsert.BirthDate,
                    Password = userInsert.Password,
                    Email = userInsert.Email,
                    PhoneNumber = userInsert.PhoneNumber
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(Messages.User.Registered);
            }
            catch (Exception ex)
            {
                return Problem(Messages.Database.ProblemRelated, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInsert loginInsert)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");

            if (string.IsNullOrEmpty(jwtKey))
            {
                return Problem(Messages.API.JWTNotConfigured);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginInsert.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginInsert.Password, user.Password))
            {
                return Unauthorized(Messages.User.WrongCredentials);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                ]),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtIssuer,
                Audience = jwtIssuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);

            return Ok(new 
            { 
                userId = user.UserId,
                username = user.Username,
                token = jwt,
            });
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInsert changePasswordInsert)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == changePasswordInsert.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(changePasswordInsert.OldPassword, user.Password))
                {
                    return Unauthorized(Messages.User.WrongCredentials);
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordInsert.NewPassword);
                await _context.SaveChangesAsync();

                return Ok(Messages.User.PasswordChanged);
            }
            catch (Exception ex)
            {
                return Problem(Messages.Database.ProblemRelated, ex.Message);
            }
        }
    }
}