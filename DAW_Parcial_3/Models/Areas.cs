using System.ComponentModel.DataAnnotations;

namespace DAW_Parcial_3.Models
{
    public class Areas
    {
        [Key]
        public int Id_area { get; set; }
        public string nombre { get; set; }
    }
}
