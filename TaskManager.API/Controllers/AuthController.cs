using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs.Auth;
using TaskManager.API.Services.Jwt;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(UserManager<IdentityUser> _userManager
        , SignInManager<IdentityUser> _signInManager, JwtService _jwtService
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
        

    }

        
    
}
