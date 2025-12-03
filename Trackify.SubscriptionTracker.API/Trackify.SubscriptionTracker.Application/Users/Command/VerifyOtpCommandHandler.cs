using FluentValidation;
using MediatR;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, bool>
    {
        private readonly IOtpService _otpService;

        public VerifyOtpCommandHandler(IOtpService otpService)
        {
            _otpService = otpService;
        }

        public async Task<bool> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            return await _otpService.VerifyOtp(request.Email, request.Otp);
        }
    }

}
