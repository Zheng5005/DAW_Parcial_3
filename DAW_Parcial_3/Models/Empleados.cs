namespace DAW_Parcial_3.Models
{
    public class Empleados
    {
        public int id_empleado { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public string telefono { get; set; }
        public string? contrasena { get; set; }
        public string rol { get; set; }
        public string? foto { get; set; }
    }
}
