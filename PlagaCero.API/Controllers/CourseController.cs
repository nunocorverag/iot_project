using Fition.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlagaCero.API.Models;

namespace PlagaCero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly AppDb _context;

        public CourseController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await _context.Courses.Include(c => c.Students)
                                                 .Select(c => new CourseDto 
                                                 { 
                                                     CourseId = c.CourseId, 
                                                     Name = c.Name 
                                                 })
                                                 .ToListAsync();
            return courses;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CourseWithStudentsDto>> GetCourse(int id)
        {
            var course = await _context.Courses.Include(c => c.Students)
                                                .Select(c => new CourseWithStudentsDto 
                                                { 
                                                    CourseId = c.CourseId, 
                                                    Name = c.Name,
                                                    Students = c.Students.Select(s => new StudentDto 
                                                    { 
                                                        StudentId = s.StudentId, 
                                                        Name = s.Name 
                                                    }).ToList() 
                                                })
                                                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            return course;
        }

        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse(CreateCourseDto createCourseDto)
        {
            if (string.IsNullOrWhiteSpace(createCourseDto.Name))
            {
                return BadRequest("El nombre es requerido.");
            }

            var course = new Course
            {
                Name = createCourseDto.Name
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            var courseDto = new CourseDto
            {
                CourseId = course.CourseId,
                Name = course.Name
            };

            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseId }, courseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, UpdateCourseDto updateCourseDto)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(updateCourseDto.Name))
            {
                return BadRequest();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            course.Name = updateCourseDto.Name;
            _context.Entry(course).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}
