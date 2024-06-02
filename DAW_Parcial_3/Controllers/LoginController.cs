using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using DAW_Parcial_3.Models;
using System.Threading.Tasks;

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

        [HttpPost]
        public async Task<IActionResult> ValidarUsuario(credenciales credenciales)
        {
            var usuario = _context.usuarios
                                  .FirstOrDefault(user => user.correo == credenciales.correo &&
                                                          user.contrasena == credenciales.contrasena);

            var empleado = _context.empleados
                                   .FirstOrDefault(em => em.correo == credenciales.correo &&
                                                         em.contrasena == credenciales.contrasena);

            if (usuario != null)
            {
                string datosUsuario = JsonSerializer.Serialize(new
                {
                    usuario.id_user,
                    usuario.correo,
                    usuario.nombre,
                    usuario.apellido,
                    Tipo = "Usuario"
                });

                HttpContext.Session.SetString("usuario", datosUsuario);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuario.correo),
                    new Claim(ClaimTypes.Role, "Usuario"),
                    new Claim("Id", usuario.id_user.ToString()),
                    new Claim("Nombre", usuario.nombre),
                    new Claim("Apellido", usuario.apellido)
                };


                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                ViewBag.Mensaje = "Bienvenido";
                return RedirectToAction("Index", "Inicio");
            }

            if (empleado != null)
            {
                string datosEmpleado = JsonSerializer.Serialize(new
                {
                    empleado.id_empleado,
                    empleado.correo,
                    empleado.nombre,
                    empleado.apellido,
                    empleado.rol,
                    Tipo = "Empleado"
                });
                HttpContext.Session.SetString("empleado", datosEmpleado);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, empleado.correo),
                    new Claim(ClaimTypes.Role, empleado.rol),
                    new Claim("Id", empleado.id_empleado.ToString()),
                    new Claim("Nombre", empleado.nombre),
                    new Claim("Apellido", empleado.apellido)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                ViewBag.Mensaje = "Bienvenido";

                if (empleado.rol == "Admin")
                {
                    return RedirectToAction("IndexAdmin", "Inicio");
                }

                return RedirectToAction("IndexEmpleado", "Inicio");
            }

            ViewBag.Mensaje = "Credenciales incorrectas!!";
            return View("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
