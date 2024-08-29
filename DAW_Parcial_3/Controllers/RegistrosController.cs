using DAW_Parcial_3.Models;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace DAW_Parcial_3.Controllers
{
    public class RegistrosController : Controller
    {
        private readonly DAWContext _context;

        public RegistrosController(DAWContext context)
        {
            _context = context;
        }

        // GET: Empleados/Create
        public IActionResult Registroem()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Registroem([Bind("id_empleado,nombre,apellido,correo,telefono,contrasena,rol,foto")] Empleados empleados, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                string child = "FotosEmpleados";
                empleados.rol = Request.Form["rol"];

                if (archivo != null && archivo.Length > 0)
                {
                    Stream archivosubido = archivo.OpenReadStream();
                    string urlarchivo = await SubirArchivo(archivosubido, archivo.FileName, child);
                    empleados.foto = urlarchivo;
                }
                _context.Add(empleados);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexAdmin", "Inicio");
            }
            return View(empleados);
        }

        // GET: /Create
        public IActionResult Registrous()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Registrous([Bind("id_user,nombre,apellido,correo,telefono,contrasena,nombre_empresa,nombre_contacto,foto")] Usuarios usuarios, IFormFile archivo)
        {
            if (ModelState.IsValid)
            {
                string child = "FotosUsuarios";

                if (archivo != null && archivo.Length > 0)
                {
                    Stream archivosubido = archivo.OpenReadStream();
                    string urlarchivo = await SubirArchivo(archivosubido, archivo.FileName, child);
                    usuarios.foto = urlarchivo;
                }
                _context.Add(usuarios);
                await _context.SaveChangesAsync();
                return RedirectToAction("IndexAdmin", "Inicio");
            }
            return View(usuarios);
        }


        // Método para subir archivo a Firebase Storage
        public async Task<string> SubirArchivo(Stream archivoSubir, string nombreArchivo,string child)
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
            ).Child(child)
            .Child(nombreArchivo)
            .PutAsync(archivoSubir, cancellation.Token);

            var urlcarga = await cargararchivo;

            return urlcarga;
        }

    }


    }

