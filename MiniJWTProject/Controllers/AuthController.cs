using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiniJWTProject.Data;
using MiniJWTProject.Dtos;
using MiniJWTProject.Entities;
using MiniJWTProject.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniJWTProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly DbConnection _dbConnection;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthManager authManager, DbConnection dbConnection, IConfiguration configuration)
        {
            _authManager = authManager;
            _dbConnection = dbConnection;
            _configuration = configuration;
        }


        [HttpPost]
        public IActionResult Register([FromBody] UserRegisterDto userRegisterDto)
        {
            if (_dbConnection.Users.Any(u => u.Email == userRegisterDto.Email))
            {
                return BadRequest("Email already exist");
            }

            var createUser = new User()
            {
                Email = userRegisterDto.Email
            };

            var createdUser = _authManager.Register(createUser, userRegisterDto.Password);

            return StatusCode(201, createdUser);

        }

        [HttpPost]
        public IActionResult Login([FromBody] UserLoginDto userLoginDto)
        {
            var user = _authManager.Login(userLoginDto.Email, userLoginDto.Password);

            if (user == null) { return Unauthorized(); }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                 {
                    new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim (ClaimTypes.Name,user.Email)
                 }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(tokenString);

        }
    }
}
