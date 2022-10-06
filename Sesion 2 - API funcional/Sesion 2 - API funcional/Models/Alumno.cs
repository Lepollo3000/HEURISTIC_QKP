using System.ComponentModel.DataAnnotations;

namespace Sesion_2___API_funcional.Models
{
    public class Alumno
    {
        [Key]
        public long Matricula { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Apellidos { get; set; } = null!;
        public long Telefono { get; set; }
        [Required]
        public int Edad { get; set; }
        public string? UrlImagen { get; set; }
        public long Adeudos { get; set; }
        public DateTime FechaDeCreacion { get; set; }
        public DateTime FechaDeModificacion { get; set; }

    }
}
