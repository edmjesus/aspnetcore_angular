using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotnetFun.API.Data;
using dotnetFun.API.Dtos;
using dotnetFun.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace dotnetFun.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            if (!string.IsNullOrEmpty(userForRegisterDto.Username))
                userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                ModelState.AddModelError("Username", "Cet identifiant existe déjà.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usrToCreate = new User
            {
                Username = userForRegisterDto.Username
            };

            var createUser = await _repo.Register(usrToCreate, userForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            var userRepo = await _repo.Login(userForLoginDto.Username, userForLoginDto.Password);

            if (userRepo == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, userRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userRepo.Username)
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new{tokenString});
        }
    }
}