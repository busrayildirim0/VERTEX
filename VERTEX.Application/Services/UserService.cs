using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using VERTEX.API.Dtos;
using VERTEX.Application.DTOs;
using VERTEX.Domain.Entities;
using VERTEX.Persistence.Context;

namespace VERTEX.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Select(u => new UserDto { Id = u.Id, Username = u.Username, Email = u.Email })
                .ToListAsync();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            return new UserDto { Id = user.Id, Username = user.Username, Email = user.Email };
        }

        public async Task<UserDto> CreateAsync(UserDto dto)
        {
            var user = new User { Username = dto.Username, Email = dto.Email };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            dto.Id = user.Id;
            return dto;
        }

        public async Task UpdateAsync(UserDto dto)
        {
            var user = await _context.Users.FindAsync(dto.Id);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.Username = dto.Username;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> RegisterUserAsync(RegisterRequestDto userDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == userDto.Email))
            {
                throw new InvalidOperationException("Bu e-posta adresi zaten kullanılıyor.");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var newUser = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                Password = passwordHash
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new UserDto { Id = newUser.Id, Username = newUser.Username, Email = newUser.Email };
        }

        public async Task<UserDto?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || string.IsNullOrEmpty(user.Password))
            {
                return null;
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null; 
            }

            return new UserDto { Id = user.Id, Username = user.Username, Email = user.Email };
        }
    }
}