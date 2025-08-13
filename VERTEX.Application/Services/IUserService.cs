using VERTEX.API.Dtos;
using VERTEX.Application.DTOs;

namespace VERTEX.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto> CreateAsync(UserDto user);
        Task UpdateAsync(UserDto user);
        Task DeleteAsync(int id);

        Task<UserDto?> AuthenticateUserAsync(string email, string password);
        Task<UserDto> RegisterUserAsync(RegisterRequestDto userDto);
    }
}