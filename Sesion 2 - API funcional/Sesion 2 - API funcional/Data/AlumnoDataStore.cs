using Sesion_2___API_funcional.Models;
using Sesion_2___API_funcional.Models.DTO;

namespace Sesion_2___API_funcional.Data
{
    public static class AlumnoDataStore
    {
        public static List<AlumnoDTO> alumnos = new List<AlumnoDTO>()
        {
            new AlumnoDTO { Matricula = 1, Nombre = "1", Apellidos = "1", Edad = 23, Telefono = 4545454 },
            new AlumnoDTO { Matricula = 2, Nombre = "2", Apellidos = "2", Edad = 23, Telefono = 4545454 },
            new AlumnoDTO { Matricula = 3, Nombre = "3", Apellidos = "3", Edad = 23, Telefono = 4545454 },
            new AlumnoDTO { Matricula = 4, Nombre = "4", Apellidos = "4", Edad = 23, Telefono = 4545454 }
        };
    }
}
