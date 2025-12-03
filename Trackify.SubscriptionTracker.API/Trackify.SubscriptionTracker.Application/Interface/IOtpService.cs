using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Application.Interface
{
    public interface IOtpService
    {
        Task<string> GenerateOtp(string email);
        Task<bool> VerifyOtp(string email, string enteredOtp);
    }
}
