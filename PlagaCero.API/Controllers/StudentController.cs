using Fition.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlagaCero.API.Models;

namespace PlagaCero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDb _context;

        public StudentController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _context.Students.Include(s => s.Courses)
                                                   .Select(s => new StudentDto 
                                                   { 
                                                       StudentId = s.StudentId, 
                                                       Name = s.Name 
                                                   })
                                                   .ToListAsync();
            return students;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentWithCoursesDto>> GetStudent(int id)
        {
            var student = await _context.Students.Include(s => s.Courses)
                                                  .Select(s => new StudentWithCoursesDto 
                                                  { 
                                                      StudentId = s.StudentId, 
                                                      Name = s.Name,
                                                      Courses = s.Courses.Select(c => new CourseDto 
                                                      { 
                                                          CourseId = c.CourseId, 
                                                          Name = c.Name 
                                                      }).ToList() 
                                                  })
                                                  .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent(CreateStudentDto createStudentDto)
        {
            if (string.IsNullOrWhiteSpace(createStudentDto.Name))
            {
                return BadRequest("El nombre es requerido.");
            }

            var student = new Student
            {
                Name = createStudentDto.Name
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            var studentDto = new StudentDto
            {
                StudentId = student.StudentId,
                Name = student.Name
            };

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, UpdateStudentDto updateStudentDto)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(updateStudentDto.Name))
            {
                return BadRequest();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            student.Name = updateStudentDto.Name;
            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
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
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}
