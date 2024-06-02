using DAW_Parcial_3.Models;
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

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") != null)
            {
                var datosUsuario = JsonSerializer.Deserialize<Usuarios>(HttpContext.Session.GetString("usuario"));
                ViewBag.NombreUsuario = datosUsuario.nombre;
            }

            return View();
        }

        public IActionResult IndexEmpleado()
        {
            if (HttpContext.Session.GetString("empleado") != null)
            {
                var datosEmpleado = JsonSerializer.Deserialize<Empleados>(HttpContext.Session.GetString("empleado"));
                ViewBag.NombreUsuario = datosEmpleado.nombre;
            }

            return View("IndexEmpleado");
        }

        public IActionResult IndexAdmin()
        {
            if (HttpContext.Session.GetString("empleado") != null)
            {
                var datosEmpleado = JsonSerializer.Deserialize<Empleados>(HttpContext.Session.GetString("empleado"));
                ViewBag.NombreUsuario = datosEmpleado.nombre;
            }

            return View("IndexEmpleado");
        }
    }
}
