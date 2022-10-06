using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sesion_2___API_funcional.Data;
using Sesion_2___API_funcional.Models;
using Sesion_2___API_funcional.Models.DTO;

namespace Sesion_2___API_funcional.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnosController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        public AlumnosController(ApplicationDbContext dbcontext, IMapper mapper) {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AlumnoDTO>))]
        public ActionResult<IEnumerable<AlumnoDTO>> Get()
        {
            List<Alumno> lstAlumnos = _dbcontext.Alumnos.OrderBy(u => u.Matricula).ToList();
            return Ok(_mapper.Map<IEnumerable<AlumnoDTO>>(lstAlumnos));
        }

        [HttpGet("{matricula}", Name = "GetAlumno")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlumnoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<AlumnoDTO> Get(long matricula)
        {
            if (matricula <= 0) return BadRequest();
            Alumno alumno = _dbcontext.Alumnos.FirstOrDefault(u => u.Matricula == matricula);
            if (alumno == null) return NotFound();

            return Ok(_mapper.Map<AlumnoDTO>(alumno));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AlumnoDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<AlumnoDTO> Post([FromBody] AlumnoCreateDTO alumno)
        {
            if(alumno == null) return BadRequest(alumno);

            Alumno a = _mapper.Map<Alumno>(alumno);
            _dbcontext.Alumnos.Add(a);
            _dbcontext.SaveChanges();

            return CreatedAtAction("Get", new { matricula = a.Matricula}, a);
        }

        [HttpPut("{matricula}")]
        public IActionResult Put(long matricula, [FromBody] AlumnoDTO alumno)
        {
            if (alumno == null || alumno.Matricula == 0) return BadRequest(alumno);

            AlumnoDTO alumnoEncontrado = AlumnoDataStore.alumnos.Where(u => u.Matricula == alumno.Matricula).FirstOrDefault();
            if(alumnoEncontrado != null)
            {
                AlumnoDataStore.alumnos.Remove(alumnoEncontrado);
                AlumnoDataStore.alumnos.Add(alumno);
            }else
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{matricula}")]
        public IActionResult Delete(long matricula)
        {
            if (matricula == 0) return BadRequest();

            AlumnoDTO alumnoEncontrado = AlumnoDataStore.alumnos.Where(u => u.Matricula == matricula).FirstOrDefault();
            if (alumnoEncontrado != null)
                AlumnoDataStore.alumnos.Remove(alumnoEncontrado);
            else
                return NotFound();

            return NoContent();
        }

    }
}
