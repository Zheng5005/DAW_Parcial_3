using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace DAW_Parcial_3.Models
{
    public class Comentarios
    {
        [Key]
        public int id_comentario { get; set; }
        public int id_ticket { get; set; }
        public string comentario { get; set; }
        public string estado { get; set; }
    }
}
