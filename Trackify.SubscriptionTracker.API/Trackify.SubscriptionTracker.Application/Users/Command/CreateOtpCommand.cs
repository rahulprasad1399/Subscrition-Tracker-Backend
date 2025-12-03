using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.Application.Users.Command
{
    public class CreateOtpCommand : IRequest<string>
    {
        public string FullName { get; set; }
        public string Email {  get; set; }
    }
    public class CreateOtpCommandHandler : IRequestHandler<CreateOtpCommand, string>
    {
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;

        public CreateOtpCommandHandler(IOtpService otpService,IEmailService emailService)
        {
            _otpService = otpService;
            _emailService = emailService;
        }

        public async Task<string> Handle(CreateOtpCommand request, CancellationToken cancellationToken)
        {
            string otp =await _otpService.GenerateOtp(request.Email);
            if (string.IsNullOrEmpty(otp))
            {
                return null;
            }

            string subject = "Trackify Verification Code";
           
            string htmlContent = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='UTF-8'>
                <title>Verification Code</title>
            </head>
            <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0;'>
                <table role='presentation' style='width: 100%; border-collapse: collapse;'>
                    <tr>
                        <td align='center' style='padding: 20px 0;'>
                            <table role='presentation' style='width: 600px; border-collapse: collapse; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.1);'>
                                <!-- Header -->
                                <tr>
                                    <td style='background-color: #004aad; padding: 20px; text-align: center;'>
                                        <h1 style='margin: 0; color: #ffffff; font-size: 24px;'>Trackify</h1>
                                    </td>
                                </tr>
                                <!-- Body -->
                                <tr>
                                    <td style='padding: 30px; color: #333333;'>
                                        <p style='margin: 0 0 15px 0; font-size: 16px;'>Hi <strong>{request.FullName}</strong>,</p>
                                        <p style='margin: 0 0 20px 0; font-size: 16px; line-height: 1.5;'>
                                            Use the One-Time Password (OTP) below to complete your verification process. 
                                            This code is valid for <strong>10 minutes</strong>.
                                        </p>
                                        
                                        <!-- OTP Box -->
                                        <div style='background-color: #f0f4f8; border: 1px dashed #004aad; border-radius: 6px; padding: 15px; text-align: center; margin: 25px 0;'>
                                            <span style='font-size: 32px; font-weight: bold; color: #004aad; letter-spacing: 5px;'>{otp}</span>
                                        </div>

                                        <p style='margin: 0 0 10px 0; font-size: 14px; color: #666666;'>
                                            If you did not request this code, please ignore this email. Do not share this code with anyone.
                                        </p>
                                    </td>
                                </tr>
                                <!-- Footer -->
                                <tr>
                                    <td style='background-color: #f9f9f9; padding: 15px; text-align: center; font-size: 12px; color: #888888;'>
                                        &copy; {DateTime.Now.Year} Trackify. All rights reserved.
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>";

            
            await _emailService.SendEmailAsync(request.Email, subject, htmlContent);

            return otp;
        }
    }
}
