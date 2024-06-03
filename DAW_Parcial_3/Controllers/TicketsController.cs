using DAW_Parcial_3.Models;
using DAW_Parcial_3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Firebase.Auth;
using Firebase.Storage;

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

        [Authorize]
        public IActionResult Index(string searchString)
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

            var DatosUsuarios = from U in _context.usuarios
                                select new
                                {
                                    id = U.id_user,
                                    nombre = U.nombre,
                                    apellido = U.apellido
                                };

            if (!String.IsNullOrEmpty(searchString))
            {
                DatosUsuarios = DatosUsuarios.Where(s => s.nombre.Contains(searchString));
            }

            ViewBag.Empleados = DatosEmpleados;
            ViewBag.Problemas = Problemas;
            ViewBag.Usuarios = DatosUsuarios.ToList();
            ViewBag.SearchString = searchString;

            Correo enviarCorreos = new Correo(_configuration);
            enviarCorreos.enviar("jorgefranciscocz@gmail.com", "Hola", "Prueva");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            ViewBag.UserId = userId;

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Tickets tickets, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                tickets.fecha_inicio = DateTime.Now;

                if (archivo != null && archivo.Length > 0)
                {
                    Stream archivosubido = archivo.OpenReadStream();
                    string urlarchivo = await SubirArchivos(archivosubido, archivo.FileName);
                    tickets.archivos = urlarchivo;
                }

                _context.Add(tickets);
                await _context.SaveChangesAsync();
                // Redirigir a la acción IndexAdmin en el controlador Inicio
                return RedirectToAction("IndexAdmin", "Inicio");
            }

            // Recarregar dados necessários para a vista Index
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

            
            ViewBag.Empleados = DatosEmpleados;
            ViewBag.Problemas = Problemas;

            // Almacenar el id del usuario en ViewBag para el formulario
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            ViewBag.UserId = userId;

            return View("Index", tickets);
        }

        //Subir archivos
        public async Task<string> SubirArchivos(Stream archivoSubir, string nombreArchivo)
        {
            string email = "";
            string clave = "";
            string ruta = "";
            string apikey = "";

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
        

        //Para el historial
        public IActionResult Historial()
        {
            var HistorialDatos = (from U in _context.usuarios
                                  join T in _context.tickets on U.id_user equals T.id_user
                                  join E in _context.empleados on T.id_empleado equals E.id_empleado into empleadoGroup
                                  from E in empleadoGroup.DefaultIfEmpty() //left join
                                  select new
                                  {
                                      id = T.id_ticket,
                                      fechaI = T.fecha_inicio,
                                      nombreServicio = T.serv_app,
                                      nombre = U.nombre,
                                      apellido = U.apellido,
                                      empleadoName = E != null ? E.nombre : null,
                                      empleadoApellido = E != null ? E.apellido : null,
                                      descripcion = T.descripcion,
                                      prioridad = T.prioridad
                                  }).ToList();

            ViewBag.Historial = HistorialDatos;
            return View(); 
        }
    }
}
