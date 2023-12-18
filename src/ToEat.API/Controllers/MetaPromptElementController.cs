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
    public class MetaPromptElementController : ControllerBase
    {
        private readonly ToEatContext _context;

        public MetaPromptElementController(ToEatContext context)
        {
            _context = context;
        }

        // GET: api/MetaPromptElement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MetaPromptElement>>> GetMetaPromptElement()
        {
          if (_context.MetaPromptElements == null)
          {
              return NotFound();
          }
            return await _context.MetaPromptElements.ToListAsync();
        }

        // GET: api/MetaPromptElement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MetaPromptElement>> GetMetaPromptElement(int id)
        {
          if (_context.MetaPromptElements == null)
          {
              return NotFound();
          }
            var metaPromptElement = await _context.MetaPromptElements.FindAsync(id);

            if (metaPromptElement == null)
            {
                return NotFound();
            }

            return metaPromptElement;
        }

        // PUT: api/MetaPromptElement/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMetaPromptElement(int id, MetaPromptElement metaPromptElement)
        {
            if (id != metaPromptElement.Id)
            {
                return BadRequest();
            }

            _context.Entry(metaPromptElement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MetaPromptElementExists(id))
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

        // POST: api/MetaPromptElement
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MetaPromptElement>> PostMetaPromptElement(MetaPromptElement metaPromptElement)
        {
          if (_context.MetaPromptElements == null)
          {
              return Problem("Entity set 'ToEatContext.MetaPromptElement'  is null.");
          }
            _context.MetaPromptElements.Add(metaPromptElement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMetaPromptElement", new { id = metaPromptElement.Id }, metaPromptElement);
        }

        // DELETE: api/MetaPromptElement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMetaPromptElement(int id)
        {
            if (_context.MetaPromptElements == null)
            {
                return NotFound();
            }
            var metaPromptElement = await _context.MetaPromptElements.FindAsync(id);
            if (metaPromptElement == null)
            {
                return NotFound();
            }

            _context.MetaPromptElements.Remove(metaPromptElement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MetaPromptElementExists(int id)
        {
            return (_context.MetaPromptElements?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
