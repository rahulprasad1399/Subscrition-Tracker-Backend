using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Interface
{
    public interface IAuthRepository
    {
        Task<User> GetByEmailAsync(string email);
        bool VerifyPassword(User user, string password);
        Task SaveRefreshTokenAsync(User user, string refreshToken);
        Task<User> GetByRefreshTokenAsync(string refreshToken);

        Task<bool> CheckEmailExistAsync(string email);
        string HashPassword(User user, string password);
        Task<int?> ValidateRefreshTokenAsync(string refreshToken);
    }
}
