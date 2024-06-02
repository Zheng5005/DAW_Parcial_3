using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using DAW_Parcial_3.Models;

namespace DAW_Parcial_3.Controllers
{
    public class LoginController : Controller
    {
        private readonly DAWContext _context;

        public LoginController(DAWContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ValidarUsuario(credenciales credenciales)
        {
            //Se busca en ambas tablas para determinar a que grupo pertenecen las credenciales
            Usuarios? usuario = (from user in _context.usuarios
                              where user.correo == credenciales.correo
                              && user.contrasena == credenciales.contrasena
                              select user).FirstOrDefault();

            Empleados? empleado = (from em in _context.empleados
                                 where em.correo == credenciales.correo
                                 && em.contrasena == credenciales.contrasena
                                 select em).FirstOrDefault();

            //Si usuario es nulo, se asume que es un empleado
            if (empleado != null)
            {
                String datosEmpleado = JsonSerializer.Serialize(empleado);
                HttpContext.Session.SetString("empleado", datosEmpleado);

                if (empleado.rol == "Admin")
                {
                    ViewBag.Mensaje = "Bienvenido";
                    return RedirectToAction("IndexAdmin", "Inicio");
                }

                return RedirectToAction("IndexEmpleado", "Inicio");
            }

            if (usuario != null) 
            {
                string datosUsuario = JsonSerializer.Serialize(usuario);
                HttpContext.Session.SetString("usuario", datosUsuario);
                ViewBag.Mensaje = "Bienvenido";

                return RedirectToAction("Index", "Inicio");
            }

            //Se usume que es un Usuario
            return View("Index");
        }
    }
}
