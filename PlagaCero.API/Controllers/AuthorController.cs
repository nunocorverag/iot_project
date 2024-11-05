using Fition.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlagaCero.API.Models;

namespace PlagaCero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly AppDb _context;

        public AuthorController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _context.Authors.Include(a => a.Books)
                                                 .Select(a => new AuthorDto 
                                                 { 
                                                     AuthorId = a.AuthorId, 
                                                     Name = a.Name 
                                                 })
                                                 .ToListAsync();
            return authors;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorWithBooksDto>> GetAuthor(int id)
        {
            var author = await _context.Authors.Include(a => a.Books)
                                                .Select(a => new AuthorWithBooksDto 
                                                { 
                                                    AuthorId = a.AuthorId, 
                                                    Name = a.Name,
                                                    Books = a.Books.Select(b => new BookDto 
                                                    { 
                                                        BookId = b.BookId, 
                                                        Title = b.Title 
                                                    }).ToList() 
                                                })
                                                .FirstOrDefaultAsync(a => a.AuthorId == id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> PostAuthor(CreateAuthorDto createAuthorDto)
        {
            if (string.IsNullOrWhiteSpace(createAuthorDto.Name))
            {
                return BadRequest("El nombre del autor es requerido.");
            }

            var author = new Author
            {
                Name = createAuthorDto.Name
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            var authorDto = new AuthorDto
            {
                AuthorId = author.AuthorId,
                Name = author.Name
            };

            return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, authorDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, UpdateAuthorDto updateAuthorDto)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(updateAuthorDto.Name))
            {
                return BadRequest();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            author.Name = updateAuthorDto.Name;
            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.AuthorId == id);
        }
    }
}
