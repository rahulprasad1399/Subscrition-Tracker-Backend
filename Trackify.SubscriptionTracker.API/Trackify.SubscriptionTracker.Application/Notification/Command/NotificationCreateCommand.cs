using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackify.SubscriptionTracker.Application.Interface;
using Trackify.SubscriptionTracker.Domain.Entity;

namespace Trackify.SubscriptionTracker.Application.Notification.Command
{
    public class NotificationCreateCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public int SubscriptionId { get; set; }
        public string ServiceName { get; set; }
        public string Subscriptyiontype { get; set; }
        public DateTime RenewalDate { get; set; }
        public decimal Cost { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

    }

    public class NotificationCreateCommandHandler : IRequestHandler<NotificationCreateCommand, bool>
    {
        private readonly IGenericRepository<UserNotification> _genericRepository;
        private readonly IEmailService _emailService;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Subscription> _subscriptionRepository;

        public NotificationCreateCommandHandler(IGenericRepository<UserNotification> genericRepository,
            IEmailService emailService, IGenericRepository<Subscription> subscriptionRepository)
        {
            _genericRepository = genericRepository;
            _emailService = emailService;
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<bool> Handle(NotificationCreateCommand request, CancellationToken cancellationToken)
        {
            
            string Title = "";
            string Message = "";

            if (request.RenewalDate < DateTime.UtcNow)
            {
                Title = "Subscription Expired";
                Message = $"Your subscription for <strong>{request.ServiceName}</strong> expired on <strong>{request.RenewalDate:dd MMM yyyy}</strong>. Please renew it to continue enjoying the service.";
            }
            else
            {
                Title = "Upcoming Subscription Renewal";
                Message = $"This is a reminder that your subscription for <strong>{request.ServiceName}</strong> will renew on <strong>{request.RenewalDate:dd MMM yyyy}</strong>. Please ensure your payment method is up to date.";
            }

            var logoUrl = "https://raw.githubusercontent.com/nabeelpp-cloud/assignment-repo/hotel-management-project/Gemini_Generated_Image_w6hgn6w6hgn6w6hg.png";

            var htmlMessage = $@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>{Title}</title>
                </head>
            <body style=""margin:0; padding:0; background-color:#f4f6f8; font-family:'Helvetica Neue', Helvetica, Arial, sans-serif; -webkit-font-smoothing:antialiased;"">

                <table role=""presentation"" width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""background-color:#f4f6f8; padding: 20px 0;"">
                    <tr>
                        <td align=""center"">
                
                            <table role=""presentation"" width=""600"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""width:100%; max-width:600px; background-color:#ffffff; border-radius:8px; box-shadow: 0 4px 6px rgba(0,0,0,0.05); overflow:hidden;"">
                    
                                <tr>
                                    <td align=""center"" style=""background-color:#004aad; padding:30px 20px;"">
                                        <img src=""{logoUrl}"" width=""80"" height=""80"" alt=""Trackify Logo"" style=""display:block; border-radius:12px; margin-bottom:15px; background-color:#ffffff; padding:5px;""/>
                                        <h1 style=""margin:0; font-size:24px; color:#ffffff; font-weight:bold; letter-spacing:0.5px;"">{Title}</h1>
                                    </td>
                                </tr>

                                <tr>
                                    <td style=""padding:40px 30px; color:#333333;"">
                                        <p style=""margin:0 0 20px 0; font-size:18px; font-weight:bold; color:#1a1a1a;"">Hi {request.UserName},</p>
                            
                                        <p style=""margin:0 0 25px 0; font-size:16px; line-height:1.6; color:#444444;"">
                                            {Message}
                                        </p>

                                        <table role=""presentation"" width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"" style=""background-color:#f8f9fa; border-radius:6px; border-left:4px solid #004aad; margin-bottom:30px;"">
                                            <tr>
                                                <td style=""padding:20px;"">
                                                    <table role=""presentation"" width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                                                        <tr>
                                                            <td style=""padding-bottom:10px; font-size:14px; color:#666666; font-weight:bold; text-transform:uppercase;"">Service Name</td>
                                                            <td align=""right"" style=""padding-bottom:10px; font-size:16px; color:#333333; font-weight:bold;"">{request.ServiceName}</td>
                                                        </tr>
                                                        <tr>
                                                            <td style=""padding-bottom:10px; font-size:14px; color:#666666; font-weight:bold; text-transform:uppercase;"">Renewal Date</td>
                                                            <td align=""right"" style=""padding-bottom:10px; font-size:16px; color:#333333;"">{request.RenewalDate:MMM dd, yyyy}</td>
                                                        </tr>
                                                        <tr>
                                                            <td style=""padding-bottom:10px; font-size:14px; color:#666666; font-weight:bold; text-transform:uppercase;"">Plan</td>
                                                            <td align=""right"" style=""padding-bottom:10px; font-size:16px; color:#333333;"">{request.Subscriptyiontype}</td>
                                                        </tr>
                                                        <tr>
                                                            <td style=""font-size:14px; color:#666666; font-weight:bold; text-transform:uppercase;"">Amount</td>
                                                            <td align=""right"" style=""font-size:20px; color:#004aad; font-weight:bold;"">₹{request.Cost}</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>

                                        <div style=""text-align:center; margin-top:10px;"">
                                            <p style=""font-size:15px; color:#666666; margin-bottom:20px;"">You can manage this subscription anytime from your dashboard.</p>
                                            <a href=""#"" style=""display:inline-block; background-color:#004aad; color:#ffffff; text-decoration:none; padding:12px 24px; border-radius:5px; font-weight:bold; font-size:16px;"">Go to Dashboard</a>
                                        </div>
                            
                                        <p style=""margin-top:40px; border-top:1px solid #eeeeee; padding-top:20px; font-size:16px; color:#333333;"">
                                            Warm regards,<br>
                                            <strong>The Trackify Team</strong>
                                        </p>
                                    </td>
                                </tr>

                                <tr>
                                    <td align=""center"" style=""background-color:#f4f6f8; padding:20px; color:#888888; font-size:12px;"">
                                        <p style=""margin:0 0 10px 0;"">© 2025 Trackify. All Rights Reserved.</p>
                                        <p style=""margin:0;"">
                                            <a href=""#"" style=""color:#888888; text-decoration:underline;"">Privacy Policy</a> | 
                                            <a href=""#"" style=""color:#888888; text-decoration:underline;"">Support</a>
                                        </p>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                </table>

            </body>
            </html>";

            var notification = new UserNotification(request.UserId, request.SubscriptionId,
                            Title, request.Type);

            Subscription subscription = await _subscriptionRepository.GetByIdAsync(request.SubscriptionId);
            if (subscription == null)
            {
                throw new KeyNotFoundException("Subscription Id dosen't exist");
            }

            await _emailService.SendEmailAsync(request.UserEmail, Title, htmlMessage);
            await _genericRepository.AddAsync(notification, cancellationToken);

            return true;
        }
    }
}
