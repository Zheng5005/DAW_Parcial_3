using System.ComponentModel.DataAnnotations;

namespace DAW_Parcial_3.Models
{
    public class Tickets
    {
        [Key]
        public int id_ticket { get; set; }
        public string serv_app {  get; set; }
        public string descripcion { get; set; }
        public string? archivos { get; set; }
        public string prioridad { get; set; }
        public int id_user { get; set; }
        public int? id_empleado { get; set; }
        public int id_area { get; set; }
        public string? progreso { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime? fecha_asig {  get; set; }
        public DateTime? fecha_fin {  get; set; }
    }
}
