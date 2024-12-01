using Digital_Archive.Dto;
using Digital_Archive.Models;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Digital_Archive.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public AuthService(AppDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<string> GetTokenAsync(logindto model)
        {
            var user = await _context.Users.
                            FirstOrDefaultAsync(x => x.Email == model.Email && x.hashed_Password == model.Password);
            if (user is null)//|| !BCrypt.Net.BCrypt.Verify(model.Password, user.hashed_Password))
            {
                return null!;
            }

            var token = GenerateJwtToken(model);
            return token;
        }



        private string GenerateJwtToken(logindto user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credientials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>()
            {
                 new Claim(ClaimTypes.Email, user.Email),
            };

            var expires = DateTime.Now.AddDays(60);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ,
                audience: _config["Jwt:Issuer"],
                claims: claims,
                expires: expires,
                signingCredentials: credientials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
