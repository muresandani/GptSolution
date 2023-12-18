using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToEat.Domain.Data;
using ToEat.Domain.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;

namespace ToEat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasePromptElementController : ControllerBase
    {
        private readonly ToEatContext _context;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public BasePromptElementController(ToEatContext context, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _context = context;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;

        }

        // GET: api/BasePromptElement
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<BasePromptElement>>> GetBasePromptElement()
        {
          if (_context.BasePromptElements == null)
          {
              return NotFound();
          }
            return await _context.BasePromptElements.ToListAsync();
        }

        // GET: api/BasePromptElement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasePromptElement>> GetBasePromptElement(int id)
        {
          if (_context.BasePromptElements == null)
          {
              return NotFound();
          }
            var basePromptElement = await _context.BasePromptElements.FindAsync(id);

            if (basePromptElement == null)
            {
                return NotFound();
            }

            return basePromptElement;
        }

        // PUT: api/BasePromptElement/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasePromptElement(int id, BasePromptElement basePromptElement)
        {
            if (id != basePromptElement.Id)
            {
                return BadRequest();
            }

            _context.Entry(basePromptElement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasePromptElementExists(id))
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

        // POST: api/BasePromptElement
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BasePromptElement>> PostBasePromptElement(BasePromptElement basePromptElement)
        {
          if (_context.BasePromptElements == null)
          {
              return Problem("Entity set 'ToEatContext.BasePromptElement'  is null.");
          }
            _context.BasePromptElements.Add(basePromptElement);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasePromptElement", new { id = basePromptElement.Id }, basePromptElement);
        }

        // DELETE: api/BasePromptElement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasePromptElement(int id)
        {
            if (_context.BasePromptElements == null)
            {
                return NotFound();
            }
            var basePromptElement = await _context.BasePromptElements.FindAsync(id);
            if (basePromptElement == null)
            {
                return NotFound();
            }

            _context.BasePromptElements.Remove(basePromptElement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasePromptElementExists(int id)
        {
            return (_context.BasePromptElements?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
