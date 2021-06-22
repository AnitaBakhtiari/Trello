using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Infra.Data;
using Infra.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infra.Services
{
    public interface IJwtService
    {
        Task<string> GetTokenAsync(ApplicationUser user);
    }


    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public JwtService(IConfiguration config, RoleManager<ApplicationRole> role, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _config = config;
            _roleManager = role;
            _userManager = userManager;
            _context = context;

        }
        public async Task<string> GetTokenAsync(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_config["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(await GetValidClaims(user)),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["JWT:Issuer"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        public async Task<List<Claim>> GetValidClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            };
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoleIds = await _context.UserRoles.Where(u => u.UserId == user.Id).Select(u => u.RoleId).ToListAsync();
            var roleNames = await _context.AppplicationRoles.Where(c => userRoleIds.Contains(c.Id)).Select(d => d.Name).ToListAsync();
            claims.AddRange(userClaims);
            foreach (var userRole in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }


            return claims;
        }
    }
}
