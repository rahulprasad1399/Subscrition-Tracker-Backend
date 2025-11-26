using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackify.SubscriptionTracker.Application.Interface
{
    public interface IJwtService
    {
        string GenerateAccessToken(int userId, string email, string role);
        string GenerateRefreshToken();
    }
}
