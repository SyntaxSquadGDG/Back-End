using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Digital_Archive.Models;
using Digital_Archive.Dto;
using Digital_Archive.Services;
using Microsoft.EntityFrameworkCore;
using Digital_Archive.Services;
namespace Digital_Archive.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly AuthService _authService;
        public UsersController(AppDbContext context, IConfiguration config, AuthService authService) {
            _config = config;
            _context = context;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] logindto model)
        {
            var user = await _context.Users.
                FirstOrDefaultAsync(x => x.Email == model.Email && x.hashed_Password == model.Password);

            if (user == null)
            {
                return Unauthorized();
            }
            var token = await _authService.GetTokenAsync(model);
            return Ok(token);
        }
        [HttpGet("test")]
        public DateTime test()
        {
            DateTime aaa= DateTime.Now;
            return aaa;
        }
    }
}
