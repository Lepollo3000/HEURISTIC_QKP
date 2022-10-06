using Microsoft.EntityFrameworkCore;
using Sesion_2___API_funcional.Models;

namespace Sesion_2___API_funcional.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Alumno> Alumnos { get; set; }
    }
}
