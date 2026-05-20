using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs.Auth;
using TaskManager.API.Services.Jwt;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserManager<IdentityUser> _userManager
        , SignInManager<IdentityUser> _signInManager,RoleManager<IdentityRole> _roleManager, JwtService _jwtService
        ) : ControllerBase
    {

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password!);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if(!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "User");
            }

            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email!);

            if(user == null)
                            {
                return Unauthorized("Invalid email or password.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password!, false);

            if(!result.Succeeded)
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new { message = $"Login Successful: {token}"});
        }

        [HttpPost("make-admin/{email}")]
        public async Task<IActionResult> MakeAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            // Create Admin role if it doesn't exist
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok("User is now an Admin");
        }


    }

        
    
}
