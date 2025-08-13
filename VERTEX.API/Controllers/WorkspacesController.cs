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
    public class WorkspacesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WorkspacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/workspaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workspace>>> GetAll()
        {
            var workspaces = await _context.Workspaces
                .Include(w => w.WorkspaceUsers)
                .ThenInclude(wu => wu.User)
                .AsNoTracking()
                .ToListAsync();

             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             var userWorkspaces = await _context.WorkspaceUsers
                 .Where(wu => wu.UserId.ToString() == userId)
                 .Select(wu => wu.Workspace)
                 .ToListAsync();
             return Ok(userWorkspaces);

           
        }

        // GET: api/workspaces/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Workspace>> GetById(int id)
        {
            var workspace = await _context.Workspaces
                .Include(w => w.WorkspaceUsers)
                .ThenInclude(wu => wu.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workspace == null)
                return NotFound();

            return Ok(workspace);
        }

        // POST: api/workspaces
        [HttpPost]
        public async Task<ActionResult<Workspace>> Create([FromBody] Workspace workspace)
        {
            if (workspace == null || string.IsNullOrWhiteSpace(workspace.Name))
            {
                return BadRequest("Workspace name is required.");
            }

            _context.Workspaces.Add(workspace);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = workspace.Id }, workspace);
        }

        // PUT: api/workspaces/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Workspace workspace)
        {
            if (id != workspace.Id)
                return BadRequest();

            _context.Entry(workspace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Workspaces.Any(w => w.Id == id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        // DELETE: api/workspaces/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workspace = await _context.Workspaces.FindAsync(id);
            if (workspace == null)
                return NotFound();

            _context.Workspaces.Remove(workspace);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}