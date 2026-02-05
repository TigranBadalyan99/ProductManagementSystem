using Core.Dtos.LoginDto;
using Core.Dtos.RegisterDto;
using Core.Entites;
using Core.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.UserName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "User");

        return Ok("User created successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);

        if (user == null)
            return Unauthorized("Invalid credentials");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
            return Unauthorized("Invalid credentials");

        var token = _tokenService.GenerateJwtToken(user);

        return Ok(new { token });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("make-admin/{userName}")]
    public async Task<IActionResult> MakeAdmin(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
            return NotFound();

        await _userManager.AddToRoleAsync(user, "Admin");

        return Ok("User promoted to Admin");
    }
}
