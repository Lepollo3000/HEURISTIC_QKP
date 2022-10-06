using AutoMapper;
using Sesion_2___API_funcional.Models;
using Sesion_2___API_funcional.Models.DTO;

namespace Sesion_2___API_funcional
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Alumno, AlumnoDTO>().ReverseMap();

            CreateMap<Alumno, AlumnoUpdateDTO>().ReverseMap();
            CreateMap<Alumno, AlumnoCreateDTO>().ReverseMap();
        }
    }
}
