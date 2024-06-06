using DAW_Parcial_3.Models;
using DAW_Parcial_3.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DAW_Parcial_3.Controllers
{
    public class AnalisisController : Controller
    {
        public readonly DAWContext _context;
        public AnalisisController(DAWContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult resumenAreas()
        {
            List<VMAreas> Lista = (from t in _context.tickets
                                   join a in _context.areas on t.id_area equals a.Id_area 
                                   group a by a.nombre into grupo
                                   orderby grupo.Count() descending
                                   select new VMAreas
                                   {
                                        nombre = grupo.Key,
                                        cantidad = grupo.Count()
                                    }).ToList(); 

            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        public IActionResult resumenTickets()
        {
            List<VMTickets> Lista = (from t in _context.tickets
                                       group t by t.prioridad into grupo
                                       orderby grupo.Count() 
                                       select new VMTickets
                                       {
                                           prioridad = grupo.Key,
                                           valor = grupo.Count(),

                                       }).ToList();

            return StatusCode(StatusCodes.Status200OK, Lista);
        }
    }
}
