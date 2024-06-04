using Microsoft.EntityFrameworkCore;

namespace DAW_Parcial_3.Models
{
    public class DAWContext : DbContext
    {
        public DAWContext(DbContextOptions<DAWContext> options) : base(options) { }

        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Empleados> empleados { get; set; }
        public DbSet<Areas> areas { get; set; }
        public DbSet<Tickets> tickets { get; set; }
        public DbSet<Comentarios> comentarios { get; set; }
    }
}
