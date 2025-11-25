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
        public int Id { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        [EmailAddress, Required, MaxLength(100)]
        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required, MaxLength(25)]
        public string Role { get; set; } = "Subscriber";
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User(string fullName, string email, string passwordHash, string role, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        private User() { }
    }
}
