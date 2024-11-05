using Fition.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlagaCero.API.Models;

namespace PlagaCero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly AppDb _context;

        public BookController(AppDb context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _context.Books.Include(b => b.Author)
                                             .Select(b => new BookDto 
                                             { 
                                                 BookId = b.BookId, 
                                                 Title = b.Title, 
                                                 AuthorId = b.AuthorId 
                                             })
                                             .ToListAsync();
            return books;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookWithAuthorDto>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.Author)
                                            .Select(b => new BookWithAuthorDto 
                                            { 
                                                BookId = b.BookId, 
                                                Title = b.Title,
                                                Author = new AuthorDto 
                                                { 
                                                    AuthorId = b.Author.AuthorId, 
                                                    Name = b.Author.Name 
                                                }
                                            })
                                            .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> PostBook(CreateBookDto createBookDto)
        {
            if (string.IsNullOrWhiteSpace(createBookDto.Title) || createBookDto.AuthorId <= 0)
            {
                return BadRequest("El título y el autor son requeridos.");
            }

            var book = new Book
            {
                Title = createBookDto.Title,
                AuthorId = createBookDto.AuthorId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var bookDto = new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                AuthorId = book.AuthorId
            };

            return CreatedAtAction(nameof(GetBook), new { id = book.BookId }, bookDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, UpdateBookDto updateBookDto)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(updateBookDto.Title) || updateBookDto.AuthorId <= 0)
            {
                return BadRequest();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = updateBookDto.Title;
            book.AuthorId = updateBookDto.AuthorId;
            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
