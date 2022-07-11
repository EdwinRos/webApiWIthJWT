using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using webApiWIthJWT.Data;

namespace webApiWIthJWT.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly databaseContext _databaseContext;

        private IConfiguration _config;

        public LoginController(IConfiguration config, databaseContext databaseContext)
        {
            _config = config;
            _databaseContext = databaseContext;
        }

        //anotancion AllowAnonymous nos permite desactivar la autenticacion a este metodo
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult logIn([FromForm] UserLogin userLogin )
        {
            
            var user = Authenticate(userLogin);
            if (user != null)
            {
                var token = Generate(user);
                var response = new { Token = token };
                return Ok(new { message = "Credenciales correctas", StatusCode = 200,  response } );
            }
            return StatusCode(400, "Credenciales incorrectas");
        }

       /*Metodo que nos permite generar un token*/
        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)

            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credential);

            return new JwtSecurityTokenHandler().WriteToken(token);
        } 

        /*Inicio de sesion de usuario en la base de datos */
        private UserModel Authenticate(UserLogin userLogin)
        {
            var usuario = _databaseContext.user.Where(
                us => us.Username == userLogin.UserName && us.Password == userLogin.Password
                ).FirstOrDefault();
            if (usuario != null)
            {
                return usuario;
            }
            return null;

        }

    }
}
