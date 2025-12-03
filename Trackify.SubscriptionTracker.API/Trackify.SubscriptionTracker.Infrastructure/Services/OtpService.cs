using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.Infrastructure.Services
{
    public class OtpService : IOtpService
    {
        private readonly IMemoryCache _memoryCache;

        public OtpService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<string> GenerateOtp(string email)
        {
            string otp = new Random().Next(1000, 9999).ToString();

            var cacheOption = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            _memoryCache.Set(email, otp, cacheOption);

            return Task.FromResult(otp);
        }

        public Task<bool> VerifyOtp(string email, string enteredOtp)
        {
            if (_memoryCache.TryGetValue(email, out var storedOtpObj))
            {
                string storedOtp = storedOtpObj as string;
                if (storedOtp == enteredOtp)
                {
                    _memoryCache.Remove(email);
                    return Task.FromResult(true); 
                }
            }

            return Task.FromResult(false); 
        }
    }
}
