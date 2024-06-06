using DAW_Parcial_3.Models;
using DAW_Parcial_3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DAW_Parcial_3.Controllers
{
    public class ManejoController : Controller
    {
        private readonly DAWContext _context;
        private readonly IConfiguration _configuration;

        public ManejoController(DAWContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var empleadoIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            int empleadoID = Convert.ToInt32(empleadoIdClaim);

            var datosE = (from t in _context.tickets
                          join u in _context.usuarios on t.id_user equals u.id_user
                          where t.id_empleado == empleadoID
                          select new
                          {
                              idT = t.id_ticket,
                              nombre = u.nombre,
                              apellido = u.apellido,
                              correo = u.correo,
                              prioridad = t.prioridad,
                              estado = t.progreso
                          }).ToList();
            ViewBag.DatosT = datosE;

            //Datos panel de info
            var estadosProgreso = new[] { "Iniciado", "Procesando", "Finalizado" };

            var datosPanelInfo = (from t in _context.tickets
                                  where t.id_empleado == empleadoID
                                  group t by t.progreso into g
                                  select new
                                  {
                                      Progreso = g.Key,
                                      Conteo = g.Count()
                                  }).ToList();

            // Añadir los estados que no tienen tickets con valor 0
            var datosPanelInfoCompleto = estadosProgreso
                .GroupJoin(datosPanelInfo,
                    estado => estado,
                    dato => dato.Progreso,
                    (estado, datos) => new { Progreso = estado, Conteo = datos.FirstOrDefault()?.Conteo ?? 0 })
                .ToList();

            ViewBag.DatosPanelInfo = datosPanelInfoCompleto;

            return View();
        }

        public async Task<IActionResult> ModDatos(int id, string correo, string comentario, string estado)
        {
            try
            {
                string asunto = "El estado de su ticket está actualmente en: " + estado;
                Iniciar(id, comentario, estado);
                EnviarCorreo(correo, asunto, comentario);
            }
            catch (Exception ex)
            {
                // Manejar la excepción adecuadamente (log, etc.)
            }

            return RedirectToAction("Index", "Manejo");
        }

        public async Task<IActionResult> ModDatosFin(int id, string correo, string comentario, string estado)
        {
            try
            {
                string asunto = "El estado de su ticket está actualmente en: " + estado;
                Iniciar(id, comentario, estado);
                EnviarCorreo(correo, asunto, comentario);
                finalizar(id);
            }
            catch (Exception ex)
            {
                // Manejar la excepción adecuadamente (log, etc.)
            }

            return RedirectToAction("Index", "Manejo");
        }

        public void Iniciar(int idticket, string comentario, string estado)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DbConnection");
                string sqlQuery = "EXEC AddComentarioToTicket @id_ticket, @comentario, @estado";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id_ticket", idticket);
                        command.Parameters.AddWithValue("@comentario", comentario);
                        command.Parameters.AddWithValue("@estado", estado);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción adecuadamente (log, etc.)
            }
        }

        public void finalizar(int idticket)
        {
            try
            {
                DateTime fechafinactual = DateTime.Now;

                string connectionString = _configuration.GetConnectionString("DbConnection");
                string sqlQuery = "UPDATE Tickets SET fecha_fin = @fechafin WHERE id_ticket = @id_ticket;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id_ticket", idticket);
                        command.Parameters.AddWithValue("@fechafin", fechafinactual);

                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción adecuadamente (log, etc.)
            }
        }


        public void EnviarCorreo(string email, string asunto, string cuerpo)
        {
            Correo enviarCorreos = new Correo(_configuration);
            enviarCorreos.Enviar(email, asunto, cuerpo);
        }
    }
}
