using FaixariT.HRMS.DBModels;
using FaixariT.HRMS.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FaixariT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, IConfiguration configuration, JwtService jwtService)
        {
            _context = context;
            _configuration = configuration;
            _jwtService = jwtService; 
        }



       

       

        [HttpPost("Userlogin")]
        public async Task<IActionResult> Userlogin([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _jwtService.GenerateToken(user.UserId.ToString(), user.Email);

            var modules = await _context.AppModule
                .Include(m => m.AppMenu)
                .ThenInclude(m => m.AppSubMenu)
                .ToListAsync();

            var menuList = modules.Select(m => new
            {
                m.ModuleId,
                m.ModuleName,
                Menus = m.AppMenu.Select(menu => new
                {
                    menu.MenuId,
                    menu.MenuName,
                    menu.Url,
                    SubMenus = menu.AppSubMenu.Select(sub => new
                    {
                        sub.SubMenuId,
                        sub.SubMenuName,
                        sub.Url
                    }).ToList()
                }).ToList()
            }).ToList();

            return Ok(new
            {
                message = "Login Successful",
                token,
                user = new { user.UserId, user.FirstName, user.LastName, user.Email },
                menuList
            });
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null || user.PasswordHash != request.Password)
                return Unauthorized(new { message = "Invalid email or password" });

            var token = GenerateJwtToken(user);
            return Ok(new { token, userId = user.UserId, name = user.FirstName });
        }

        private string GenerateJwtToken(Users user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("menu")]
        public async Task<IActionResult> GetMenu()
        {
            var modules = await _context.AppModule
                .Include(m => m.AppMenu)
                .ThenInclude(menu => menu.AppSubMenu)
                .ToListAsync();

            var response = modules.Select(module => new
            {
                module.ModuleId,
                module.ModuleName,
                Menus = module.AppMenu.Select(menu => new
                {
                    menu.MenuId,
                    menu.MenuName,
                    menu.Url,
                    SubMenus = menu.AppSubMenu.Select(sub => new
                    {
                        sub.SubMenuId,
                        sub.SubMenuName,
                        sub.Url
                    }).ToList()
                }).ToList()
            });

            return Ok(response);
        }
    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
