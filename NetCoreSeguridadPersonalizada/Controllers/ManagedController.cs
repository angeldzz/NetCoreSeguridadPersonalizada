using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace NetCoreSeguridadPersonalizada.Controllers
{
    public class ManagedController : Controller
    {
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            if (username.ToLower() == "admin" && password == "12345")
            {
                //POR MEDIDAS DE SEGUIRDAD, SE GENERA UNA COOKIE CIFRADA
                //QUE ES PARA SABER SI EL USER SE HA VALIDADO EN ESTE EXPLORADOR
                //O NO PARA EVITAR EL SESSIONHIXAQUING
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role
                        );
                //UN CLAIM ES INFORMACION DEL USUARIO
                Claim claimUserName =
                    new Claim(ClaimTypes.Name, username);
                Claim claimRole =
                    new Claim(ClaimTypes.Role, "USUARIO");
                identity.AddClaim(claimUserName);
                identity.AddClaim(claimRole);
                //CREAMOS UN USUARIO PRICIPAL CON ESTA IDENTIDAD
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                //DAMOS DE ALTRA AL USER EN EL SISTEMA
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.Now.AddMinutes(10),
                    });
                return RedirectToAction("Perfil", "Usuarios");
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario o contraseña incorrectos";
                return View();
            }
        }
    }
}
