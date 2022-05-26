using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProyectoDPWA_Citas.Data;
using ProyectoDPWA_Citas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProyectoDPWA_Citas.Controllers
{
    public class AccesoController : Controller
    {

        private readonly ClinicaModContext _context;

        public AccesoController(ClinicaModContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario _usuario)
        {
            var user = _context.Usuarios.Where(p => p.Username == _usuario.Username && p.Contraseña == _usuario.Contraseña).FirstOrDefault();
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                };
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Username));
                claims.Add(new Claim(ClaimTypes.Role, user.TipoUsuario));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                if (user.TipoUsuario == "secretaria")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (user.TipoUsuario == "doctor")
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");

            }
            else
            {
                TempData["Message"] = "Las credenciales ingresadas son invalidas, intente de nuevo";
                return View();
            }
        }
        public async Task<IActionResult> SalirAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

    }
}
