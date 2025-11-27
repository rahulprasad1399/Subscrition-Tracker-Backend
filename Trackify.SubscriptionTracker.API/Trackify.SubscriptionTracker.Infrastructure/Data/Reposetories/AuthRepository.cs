using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IApplicationDbContext _context;
        public AuthRepository(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync((user) => user.Email == email);
        }

        public async Task<User> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync((user) => user.RefreshToken == refreshToken);
        }

        public string HashPassword(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.HashPassword(user, password);
        }

        public async Task SaveRefreshTokenAsync(User user, string refreshToken)
        {

            user.AddRefreshToken(refreshToken, DateTime.UtcNow.AddMinutes(10));
            await _context.SaveChangesAsync();
        }

        public async Task<User> ValidateRefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync((user) => user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.UtcNow);
            return user;
        }

        public bool VerifyPassword(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }


    }
}
