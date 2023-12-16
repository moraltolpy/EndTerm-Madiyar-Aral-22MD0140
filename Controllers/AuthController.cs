using Microsoft.AspNetCore.Mvc;
using EndTerm.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly string _secretKey;

    public AuthController(IConfiguration configuration, DataContext context)
    {
        _secretKey = configuration["SECRET_KEY"] ?? "DevelopmentKey";
        _context = context;
    }

    // POST: api/Auth/register
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Login == user.Login))
        {
            return BadRequest("Username already taken");
        }
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }

        user.Password = string.Empty;

        return Ok(user);
    }


    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == userDto.Login);

        if (user == null)
        {
            return Unauthorized("User not found.");
        }
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);

        if (!isPasswordValid)
        {
            return Unauthorized("Invalid password.");
        }

        string token = GenerateJwtToken(user);

        return Ok(token);
    }
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.Login)
        }),
            Expires = DateTime.UtcNow.AddDays(7), 
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
