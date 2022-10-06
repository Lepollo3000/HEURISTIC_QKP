using System.ComponentModel.DataAnnotations;

namespace Sesion_2___API_funcional.Models.DTO
{
    public class AlumnoCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = null!;
        [Required]
        [MaxLength(50)]
        public string Apellidos { get; set; } = null!;
        public long Telefono { get; set; }
        [Required]
        [Range(17, 99)]
        public int Edad { get; set; }
        public string? UrlImagen { get; set; }
    }
}
