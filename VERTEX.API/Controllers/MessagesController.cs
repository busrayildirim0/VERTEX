using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VERTEX.Domain.Entities;
using VERTEX.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace VERTEX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/messages 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetAll()
        {
            var messages = await _context.Messages
                .Include(m => m.Channel)
                .Include(m => m.User)
                .AsNoTracking() 
                .ToListAsync();

            return Ok(messages);
        }

        // GET: api/messages/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetById(int id)
        {
            var message = await _context.Messages
                .Include(m => m.Channel)
                .Include(m => m.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (message == null)
                return NotFound();


            return Ok(message);
        }

        // POST: api/messages
        [HttpPost]
        public async Task<ActionResult<Message>> Create([FromBody] Message message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Content))
            {
                return BadRequest("Message content is required.");
            }

           

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = message.Id }, message);
        }

        // PUT: api/messages/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Message message)
        {
            if (id != message.Id)
                return BadRequest();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             if (message.UserId.ToString() != userId)
             {
                 return Forbid();
             }

            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/messages/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
                return NotFound();

             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             if (message.UserId.ToString() != userId)
             {
                 return Forbid();
             }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}