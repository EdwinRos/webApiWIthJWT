using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using webApiWIthJWT.Data;

namespace webApiWIthJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {

        
        [HttpGet("Admins")]
        [Authorize] //validando autorizacion para ingresar a este endpoint
        public IActionResult adminEndPoint()
        {
            var currentUser = getCurrentUserInfo();
            if(currentUser.Role != "administrator")//validacion 'manual' puede hacerse por medio de anotaciones
            {
                return Ok(new { message = "No es administrador", statusCode = 200 });
            }
            return Ok(currentUser);
        }

        //metodo publico al no tener la anotacion [Authorize]
        [HttpGet("publico")]
        public IActionResult publicMethod(string palabra)
        { 
            return Ok($"Metodo publico {palabra}");
        }

        //con este metodo podemos obtener la informacion que guardamos al momento de iniciar sesion
        //de esta forma no tendriamos que hacer consultas nuevamente a la bd para trabajar con la informacio
        //del usuario loggeado


        private UserModel getCurrentUserInfo()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            if (user != null)
            {
                var userClaims = user.Claims;
                return new UserModel
                {
                    Username = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier)?.Value,
                    EmailAddress = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.Email)?.Value,
                    GivenName = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.GivenName)?.Value,
                    Surname = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.Surname)?.Value,
                    Role = userClaims.FirstOrDefault(u => u.Type == ClaimTypes.Role)?.Value

                };
            }
            return null;
        }
    }
}
