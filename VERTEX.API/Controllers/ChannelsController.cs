using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using VERTEX.Application.DTOs;
using VERTEX.Application.Services;

namespace VERTEX.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class ChannelsController : ControllerBase
    {
        private readonly IChannelService _channelService;

        public ChannelsController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        // GET: api/channels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChannelDto>>> GetAll()
        {
            var channels = await _channelService.GetAllAsync();
            return Ok(channels);
        }

        // GET: api/channels/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ChannelDto>> GetById(int id)
        {
            var channel = await _channelService.GetByIdAsync(id);
            if (channel == null)
                return NotFound(new { message = "Channel not found." });

            return Ok(channel);
        }

        // GET: /api/workspaces/{workspaceId}/channels
        [HttpGet("/api/workspaces/{workspaceId}/channels")]
        public async Task<ActionResult<IEnumerable<ChannelDto>>> GetByWorkspace(int workspaceId)
        {
            try
            {
                var channels = await _channelService.GetChannelsByWorkspaceIdAsync(workspaceId);
                return Ok(channels);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // POST: /api/workspaces/{workspaceId}/channels
        [HttpPost("/api/workspaces/{workspaceId}/channels")]
        public async Task<ActionResult<ChannelDto>> CreateInWorkspace(int workspaceId, [FromBody] ChannelDto channelDto)
        {
            try
            {
                var createdChannel = await _channelService.CreateChannelAsync(workspaceId, channelDto);
                return CreatedAtAction(nameof(GetById), new { id = createdChannel.Id }, createdChannel);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT: api/channels/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ChannelDto channelDto)
        {
            if (id != channelDto.Id)
                return BadRequest(new { message = "Invalid channel data." });

            try
            {
                await _channelService.UpdateChannelAsync(channelDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // DELETE: api/channels/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _channelService.DeleteChannelAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}