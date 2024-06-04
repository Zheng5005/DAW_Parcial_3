using DAW_Parcial_3.Models;
using Microsoft.AspNetCore.Mvc;

namespace DAW_Parcial_3.Controllers
{
    public class ManejoController : Controller
    {
        public readonly DAWContext _context;
        private readonly IConfiguration _configuration;

        public ManejoController(DAWContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var EmpleadoIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            int empleadoID = Convert.ToInt32(EmpleadoIdClaim);
            
            var datosE = (from t in _context.tickets 
                          join u in _context.usuarios on t.id_user equals u.id_user 
                          where t.id_empleado == empleadoID
                          select new
                          {
                              idT = t.id_ticket,
                              nombre = u.nombre,
                              apellido = u.apellido,
                              prioridad = t.prioridad
                          }).ToList();

            ViewBag.DatosT = datosE;

            return View();
        }
    }
}
