using DAW_Parcial_3.Models;
using DAW_Parcial_3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.EntityFrameworkCore;

namespace DAW_Parcial_3.Controllers
{
    public class TicketsController : Controller
    {
        public readonly DAWContext _context;
        private readonly IConfiguration _configuration;

        public TicketsController(DAWContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var DatosEmpleados = (from E in _context.empleados
                                  select new
                                  {
                                      id = E.id_empleado,
                                      nombre = E.nombre,
                                      apellido = E.apellido,
                                      correo = E.correo,
                                      telefono = E.telefono,
                                      rol = E.rol
                                  }).ToList();

            var Problemas = (from A in _context.areas
                             select new
                             {
                                 id = A.Id_area,
                                 nombre = A.nombre
                             }).ToList();

            var DatosUsuarios = (from U in _context.usuarios
                                select new
                                {
                                    id = U.id_user,
                                    nombre = U.nombre,
                                    apellido = U.apellido
                                }).ToList();

            ViewBag.Empleados = DatosEmpleados;
            ViewBag.Problemas = Problemas;
            ViewBag.Usuario = DatosUsuarios;

            //Correo enviarCorreos = new Correo(_configuration);
            //enviarCorreos.enviar("jorgefranciscocz@gmail.com", "Hola", "Prueva");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            ViewBag.UserId = userId;

            return View();
        }

        public IActionResult Index1()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            int userid = Convert.ToInt32(userId);

            var Problemas = (from A in _context.areas
                             select new
                             {
                                 id = A.Id_area,
                                 nombre = A.nombre
                             }).ToList();

            var DatosUsuarios = (from U in _context.usuarios
                                where U.id_user == userid
                                select new 
                                {
                                    id = U.id_user,
                                    nombre = U.nombre,
                                    apellido = U.apellido
                                }).FirstOrDefault();


            ViewBag.Problemas = Problemas;
            ViewBag.usuario = DatosUsuarios;
            ViewBag.userId = userid;

            return View();
        }


        public IActionResult SearchUsuarios(string searchString)
        {
            var resultados = from U in _context.usuarios
                             where U.nombre.Contains(searchString) || U.apellido.Contains(searchString)
                             select new
                             {
                                 id = U.id_user,
                                 nombre = U.nombre,
                                 apellido = U.apellido
                             };

            return Json(resultados.ToList());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Tickets tickets, IFormFile archivo, string idUser)
        {
            if (ModelState.IsValid)
            {
                string estado = "No Asignado";
                tickets.fecha_inicio = DateTime.Now;
                tickets.id_user = int.Parse(idUser);
                tickets.progreso = estado;

                if (archivo != null && archivo.Length > 0)
                {
                    Stream archivosubido = archivo.OpenReadStream();
                    string urlarchivo = await SubirArchivos(archivosubido, archivo.FileName);
                    tickets.archivos = urlarchivo;
                }

                _context.Add(tickets);
                await _context.SaveChangesAsync();

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var rol = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (rol == "Usuario")
                {
                    return RedirectToAction("Index", "Inicio");
                }
                else if (rol == "Empleado")
                {
                    return RedirectToAction("IndexEmpleado", "Inicio");
                }
                else if (rol == "Admin")
                {
                    return RedirectToAction("IndexAdmin", "Inicio");
                }
                else
                {
                    return BadRequest();
                }
            }

            return View("Index", tickets);
        }

        //Subir archivos
        public async Task<string> SubirArchivos(Stream archivoSubir, string nombreArchivo)
        {
            string email = "jorgefranciscocz@gmail.com";
            string clave = "ContraseñaXDXD";
            string ruta = "desarolloweb-7ffb8.appspot.com";
            string apikey = "AIzaSyBbIwF8pmsda6lLtldYsro7e_Aa_SCNGq0";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(apikey));
            var autentificar = await auth.SignInWithEmailAndPasswordAsync(email, clave);
            var cancellation = new CancellationTokenSource();
            var tokenuser = autentificar.FirebaseToken;

            var cargararchivo = new FirebaseStorage(ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(tokenuser),
                    ThrowOnCancel = true
                }
            ).Child("UsarioDatos")
            .Child(nombreArchivo)
            .PutAsync(archivoSubir, cancellation.Token);

            var urlcarga = await cargararchivo;

            return urlcarga;
        }


        [Authorize]
        public IActionResult Historial()
        {
            if (User.Identity.IsAuthenticated)
            {
                var rol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                int userId;

                if (userIdClaim == null || !int.TryParse(userIdClaim, out userId))
                {
                    return RedirectToAction("Index", "Inicio");
                }

                if (rol == "Usuario")
                {
                    var historial = (from t in _context.tickets
                                     join u in _context.usuarios on t.id_user equals u.id_user
                                     join e in _context.empleados on t.id_empleado equals e.id_empleado into empleadoGroup
                                     from e in empleadoGroup.DefaultIfEmpty()
                                     where u.id_user == userId
                                     select new
                                     {
                                         id = t.id_ticket,
                                         fechaInicio = t.fecha_inicio,
                                         servicio = t.serv_app,
                                         nombre = u.nombre,
                                         apellido = u.apellido,
                                         descripcion = t.descripcion,
                                         prioridad = t.prioridad,
                                         empleadoName = e != null ? e.nombre : null,
                                         empleadoApellido = e != null ? e.apellido : null
                                     }).ToList();

                    ViewBag.Rol = rol;
                    ViewBag.Historial = historial;
                    return View();
                }
                else if (rol == "Empleado")
                {
                    var historialEmpleado = (from t in _context.tickets
                                             join u in _context.usuarios on t.id_user equals u.id_user
                                             join e in _context.empleados on t.id_empleado equals e.id_empleado into empleadoGroup
                                             from e in empleadoGroup.DefaultIfEmpty()
                                             where t.id_empleado == userId
                                             select new
                                             {
                                                 id = t.id_ticket,
                                                 fechaInicio = t.fecha_inicio,
                                                 servicio = t.serv_app,
                                                 nombre = u.nombre,
                                                 apellido = u.apellido,
                                                 descripcion = t.descripcion,
                                                 prioridad = t.prioridad,
                                                 empleadoName = e != null ? e.nombre : null,
                                                 empleadoApellido = e != null ? e.apellido : null
                                             }).ToList();

                    ViewBag.Rol = rol;
                    ViewBag.Historial = historialEmpleado;
                    return View();
                }
                else if (rol == "Admin")
                {
                    var historialadmin = (from t in _context.tickets
                                          join u in _context.usuarios on t.id_user equals u.id_user
                                          join e in _context.empleados on t.id_empleado equals e.id_empleado into empleadoGroup
                                          from e in empleadoGroup.DefaultIfEmpty()
                                          select new
                                          {
                                              id = t.id_ticket,
                                              fechaInicio = t.fecha_inicio,
                                              servicio = t.serv_app,
                                              nombre = u.nombre,
                                              apellido = u.apellido,
                                              descripcion = t.descripcion,
                                              prioridad = t.prioridad,
                                              empleadoName = e != null ? e.nombre : null,
                                              empleadoApellido = e != null ? e.apellido : null,
                                              fechafin = t != null ? t.fecha_fin : null
                                          }).ToList();

                    ViewBag.Rol = rol;
                    ViewBag.Historial = historialadmin;
                    return View();
                }
            }

            return RedirectToAction("Index", "Inicio");
        }

        //Gestion de tickets
        [Authorize]
        [HttpGet]
        public IActionResult Gestion()
        {
            var rol = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (rol == "Admin")
            {
                var ticketAdmin = (from t in _context.tickets
                                   join e in _context.empleados on t.id_empleado equals e.id_empleado into empleadogroup
                                   from e in empleadogroup.DefaultIfEmpty()
                                   join c in _context.comentarios on t.id_ticket equals c.id_ticket into comentariogroup
                                   from c in comentariogroup.DefaultIfEmpty()
                                   select new
                                   {
                                       id = t.id_ticket,
                                       empleadoN = e != null ? e.nombre : null,
                                       empleadoA = e != null ? e.apellido : null,
                                       comentario = c != null ? c.comentario : null,
                                       progreso = t != null ? t.progreso : null,
                                       prioridad = t != null ? t.prioridad : null
                                   }).ToList();

                var empleados = (from e in _context.empleados
                                 where e.rol == "Empleado"
                                 select e).ToList();

                ViewBag.Rol = rol;
                ViewBag.Empleados = empleados;
                ViewBag.Administracion = ticketAdmin;
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ModEmpleado (Tickets tickets, int idticket, int idempleado)
        {
            var ticket = await _context.tickets.FirstOrDefaultAsync(t => t.id_ticket == idticket);
            if (ticket == null)
            {
                return BadRequest();
            }

            ticket.id_empleado = idempleado;

            _context.Update(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction("Gestion", "Tickets");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> changePriority(string prioridad, int idticket)
        {

            var ticket = await _context.tickets.FirstOrDefaultAsync(t => t.id_ticket == idticket);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.prioridad = prioridad;

            _context.Update(ticket);
            await _context.SaveChangesAsync();

            return RedirectToAction("Gestion", "Tickets");
        }
    }
}
