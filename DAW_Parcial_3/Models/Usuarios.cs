﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAW_Parcial_3.Models
{
    public class Usuarios
    {
        [Key]
        public int id_user { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set;}
        public string correo { get; set;}
        public string telefono { get; set;}
        [JsonIgnore]
        public string? contrasena { get; set;}
        public string? nombre_empresa { get; set; }
        public string? nombre_contacto { get; set; }
        public string? foto { get; set;}
    }
}
