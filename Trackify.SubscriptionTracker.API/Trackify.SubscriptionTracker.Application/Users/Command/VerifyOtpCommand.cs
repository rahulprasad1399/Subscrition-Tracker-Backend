using MediatR;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class VerifyOtpCommand : IRequest<bool>
    {
        public string Otp {  get; set; }
        public string Email { get; set; }
    }
}
