using AutoMapper;
using JWT_Practice.Data;
using JWT_Practice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWT_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // Databse Contect Class Connection
        private readonly ApplicationDbContext _context;

        //Identity Setup
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        //Mapper 
        private readonly IMapper _mapper;

        public AuthController(IConfiguration configuration, ApplicationDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserDTO request)
        {

            var IsExist = await _userManager.FindByEmailAsync(request.UserName);
            if(IsExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });
            var appUser = new AppUser()
            {
                Email = request.UserName,
                UserName=request.UserName,
            };

            var userResult = await _userManager.CreateAsync(appUser,request.Password);
            if(!userResult.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            if (await _roleManager.RoleExistsAsync("Admin"))
            {
                await _userManager.AddToRoleAsync(appUser,"Admin");
                await _userManager.AddClaimAsync(appUser, new Claim(ClaimTypes.Role,"Admin"));
                
            }
           

            return Ok(new { Status = "Success", Message = "User created successfully!" });

        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {

            var userResult= await _userManager.FindByEmailAsync(request.UserName);
            if(userResult!=null&& await _userManager.CheckPasswordAsync(userResult,request.Password))
            {
                string token = CreatToken(userResult);
                return Ok(token);
            }

            return Unauthorized();
        }

        //JWT Token
        private string CreatToken(AppUser user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                 new Claim(ClaimTypes.Role, "Admin")
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JsonWebTokenKeys:IssuerSigningKey").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(
                issuer: _configuration.GetSection("JsonWebTokenKeys:ValidIssuer").Value,
                audience: _configuration.GetSection("JsonWebTokenKeys:ValidAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


        [HttpGet("Users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserView>>> AllUsers()
        {
            List<AppUser> users = _userManager.Users.ToList();
            return Ok(users.Select(user => _mapper.Map<UserView>(user)));
        }
    }
}
