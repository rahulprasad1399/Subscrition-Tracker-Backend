using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Domain.Entity
{
    public class User
    {
        public int Id { get; private set; }
        [MaxLength(100)]
        public string FullName { get; private set; }
        [EmailAddress, Required, MaxLength(100)]
        public string Email { get; private set; }
        [Required]
        public string PasswordHash { get; private set; }
        [Required, MaxLength(25)]
        public string Role { get; private set; }
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public User(string fullName, string email, string passwordHash, string role)
        {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        public void AddRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public void UpdateUser(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }

        public void SetPaswordHash(string hash)
        {
            PasswordHash = hash;
        }

        private User() { }
    }
}
