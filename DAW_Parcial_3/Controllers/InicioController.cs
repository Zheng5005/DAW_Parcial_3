using DAW_Parcial_3.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DAW_Parcial_3.Controllers
{
    public class InicioController : Controller
    {
        private readonly DAWContext _context;

        public InicioController(DAWContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") != null)
            {
                var datosUsuario = JsonSerializer.Deserialize<Usuarios>(HttpContext.Session.GetString("usuario"));
                ViewBag.NombreUsuario = datosUsuario.nombre;
            }

            return View();
        }

        [Authorize]
        public IActionResult IndexEmpleado()
        {
            if (HttpContext.Session.GetString("empleado") != null)
            {
                var datosEmpleado = JsonSerializer.Deserialize<Empleados>(HttpContext.Session.GetString("empleado"));
                ViewBag.NombreUsuario = datosEmpleado.nombre;
            }

            return View("IndexEmpleado");
        }

        [Authorize]
        public IActionResult IndexAdmin()
        {
            if (HttpContext.Session.GetString("empleado") != null)
            {
                var datosEmpleado = JsonSerializer.Deserialize<Empleados>(HttpContext.Session.GetString("empleado"));
                ViewBag.NombreUsuario = datosEmpleado.nombre;
            }

            return View("IndexAdmin");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
