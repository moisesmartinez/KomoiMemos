using FinanceMemos.API.Data;
using FinanceMemos.API.DTOs;
using FinanceMemos.API.Helpers;
using FinanceMemos.API.Models;
using FinanceMemos.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceMemos.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly KomoiMemosDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(KomoiMemosDbContext context, IConfiguration configuration, JwtTokenService jwtTokenService)
    {
        _context = context;
        _configuration = configuration;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = new User
        {
            Username = model.Username,
            PasswordHash = PasswordHasher.HashPassword(model.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

        if (user != null && PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
        {
            // Generate and return a JWT token
            return Ok(new { Message = "Login successful" });
        }

        return Unauthorized();
    }
}