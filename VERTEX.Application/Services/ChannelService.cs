using Microsoft.EntityFrameworkCore;
using VERTEX.Application.DTOs;
using VERTEX.Domain.Entities;
using VERTEX.Persistence.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VERTEX.Application.Services
{
    public class ChannelService : IChannelService
    {
        private readonly ApplicationDbContext _context;

        public ChannelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ChannelDto>> GetAllAsync()
        {
            return await _context.Channels
                .AsNoTracking()
                .Select(c => new ChannelDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    WorkspaceId = c.WorkspaceId
                })
                .ToListAsync();
        }

        public async Task<ChannelDto?> GetByIdAsync(int id)
        {
            var channel = await _context.Channels
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (channel == null)
            {
                return null;
            }

            return new ChannelDto
            {
                Id = channel.Id,
                Name = channel.Name,
                WorkspaceId = channel.WorkspaceId
            };
        }

        public async Task<IEnumerable<ChannelDto>> GetChannelsByWorkspaceIdAsync(int workspaceId)
        {
            var workspaceExists = await _context.Workspaces.AnyAsync(w => w.Id == workspaceId);
            if (!workspaceExists)
            {
                throw new KeyNotFoundException("Workspace not found.");
            }

            return await _context.Channels
                .Where(c => c.WorkspaceId == workspaceId)
                .AsNoTracking()
                .Select(c => new ChannelDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    WorkspaceId = c.WorkspaceId
                })
                .ToListAsync();
        }

        public async Task<ChannelDto> CreateChannelAsync(int workspaceId, ChannelDto channelDto)
        {
            if (string.IsNullOrWhiteSpace(channelDto.Name))
            {
                throw new InvalidOperationException("Channel name is required.");
            }

            var workspaceExists = await _context.Workspaces.AnyAsync(w => w.Id == workspaceId);
            if (!workspaceExists)
            {
                throw new KeyNotFoundException("Workspace not found.");
            }

            var channel = new Channel
            {
                Name = channelDto.Name,
                WorkspaceId = workspaceId
            };

            _context.Channels.Add(channel);
            await _context.SaveChangesAsync();

            channelDto.Id = channel.Id;
            channelDto.WorkspaceId = channel.WorkspaceId;
            return channelDto;
        }

        public async Task UpdateChannelAsync(ChannelDto channelDto)
        {
            var channel = await _context.Channels.FindAsync(channelDto.Id);
            if (channel == null)
            {
                throw new KeyNotFoundException("Channel not found.");
            }

            channel.Name = channelDto.Name;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteChannelAsync(int id)
        {
            var channel = await _context.Channels.FindAsync(id);
            if (channel == null)
            {
                throw new KeyNotFoundException("Channel not found.");
            }

            _context.Channels.Remove(channel);
            await _context.SaveChangesAsync();
        }
    }
}