using DAW_Parcial_3.Models;
using DAW_Parcial_3.Services;
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

        public IActionResult Index()
        {
            Correo enviarCorreos = new Correo(_configuration);
            //Correo, Cuerpo, Asunto
            enviarCorreos.enviar("jorgefranciscocz@gmail.com", "Hola", "Prueva"); 

            return View();
        }
    }
}
