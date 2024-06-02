using DAW_Parcial_3.Models;
using DAW_Parcial_3.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DAW_Parcial_3.Controllers
{
    public class TicketsController : Controller
    {
        public readonly DAWContext _context;
        private IConfiguration _configuration;

        public TicketsController(DAWContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [Authorize]
        public IActionResult Index()
        {
            var DatosEmpleados = (from E in _context.empleados
                                  select new
                                  {
                                      id = E.id_empleado,
                                      nombre = E.nombre,
                                      apellido = E.apellido,
                                      Correo = E.correo,
                                      telefono = E.telefono,
                                      rol = E.rol
                                  }).ToList();

            var Problemas = (from A in _context.areas 
                             select new 
                             { 
                                id = A.Id_area,
                                nombre = A.nombre
                             });

            ViewBag.Empleados = DatosEmpleados;
            ViewBag.Problemas = Problemas;
            
            Correo enviarCorreos = new Correo(_configuration);
            
            
            //Correo, Cuerpo, Asunto
            enviarCorreos.enviar("jorgefranciscocz@gmail.com", "Hola", "Prueva"); 

            return View();
        }
    }
}
