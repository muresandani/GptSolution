using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToEat.Domain.Data;
using ToEat.Domain.Models;

namespace ToEat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OptionalPromptElementController : ControllerBase
    {
        private readonly ToEatContext _context;

        public OptionalPromptElementController(ToEatContext context)
        {
            _context = context;
        }

        // GET: api/OptionalPromptElement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OptionalPromptElement>>> GetOptionalPromptElement()
        {
          if (_context.OptionalPromptElements == null)
          {
              return NotFound();
          }
            return await _context.OptionalPromptElements.ToListAsync();
        }

        // GET: api/OptionalPromptElement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OptionalPromptElement>> GetOptionalPromptElement(int id)
        {
          if (_context.OptionalPromptElements == null)
          {
              return NotFound();
          }
            var optionalPromptElement = await _context.OptionalPromptElements.FindAsync(id);

            if (optionalPromptElement == null)
            {
                return NotFound();
            }

            return optionalPromptElement;
        }

        // PUT: api/OptionalPromptElement/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOptionalPromptElement(int id, OptionalPromptElement optionalPromptElement)
        {
            if (id != optionalPromptElement.Id)
            {
                return BadRequest();
            }

            _context.Entry(optionalPromptElement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OptionalPromptElementExists(id))
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

        // POST: api/OptionalPromptElement
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<OptionalPromptElement>> PostOptionalPromptElement(OptionalPromptElement optionalPromptElement)
        {
          if (_context.OptionalPromptElements == null)
          {
              return Problem("Entity set 'ToEatContext.OptionalPromptElement'  is null.");
          }
            _context.OptionalPromptElements.Add(optionalPromptElement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOptionalPromptElement", new { id = optionalPromptElement.Id }, optionalPromptElement);
        }

        // DELETE: api/OptionalPromptElement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOptionalPromptElement(int id)
        {
            if (_context.OptionalPromptElements == null)
            {
                return NotFound();
            }
            var optionalPromptElement = await _context.OptionalPromptElements.FindAsync(id);
            if (optionalPromptElement == null)
            {
                return NotFound();
            }

            _context.OptionalPromptElements.Remove(optionalPromptElement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OptionalPromptElementExists(int id)
        {
            return (_context.OptionalPromptElements?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
