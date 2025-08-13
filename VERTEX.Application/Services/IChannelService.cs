using VERTEX.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VERTEX.Application.Services
{
    public interface IChannelService
    {
        Task<IEnumerable<ChannelDto>> GetAllAsync();
        Task<ChannelDto?> GetByIdAsync(int id);
        Task<IEnumerable<ChannelDto>> GetChannelsByWorkspaceIdAsync(int workspaceId);
        Task<ChannelDto> CreateChannelAsync(int workspaceId, ChannelDto channelDto);
        Task UpdateChannelAsync(ChannelDto channelDto);
        Task DeleteChannelAsync(int id);
    }
}