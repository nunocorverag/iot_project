using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlagaCero.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlagaCero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaizController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MaizController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Maiz>>> GetMaiz()
        {
            return await _context.Maiz.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Maiz>> GetMaiz(int id)
        {
            var maiz = await _context.Maiz.FindAsync(id);
            if (maiz == null)
            {
                return NotFound();
            }
            return maiz;
        }

        [HttpPost]
        public async Task<ActionResult<Maiz>> PostMaiz(Maiz maiz)
        {
            _context.Maiz.Add(maiz);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMaiz), new { id = maiz.Id }, maiz);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaiz(int id, Maiz maiz)
        {
            if (id != maiz.Id)
            {
                return BadRequest();
            }

            _context.Entry(maiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Maiz.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaiz(int id)
        {
            var maiz = await _context.Maiz.FindAsync(id);
            if (maiz == null)
            {
                return NotFound();
            }

            _context.Maiz.Remove(maiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
