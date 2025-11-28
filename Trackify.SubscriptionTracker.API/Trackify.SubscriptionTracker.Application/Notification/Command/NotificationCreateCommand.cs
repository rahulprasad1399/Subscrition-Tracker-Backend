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
        public DateTime RenewalDate {  get; set; }
        public decimal Cost { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }

    public class NotificationCreateCommandHandler : IRequestHandler<NotificationCreateCommand, bool>
    {
        private readonly IGenericRepository<UserNotification> _genericRepository;
        private readonly IEmailService _emailService;
        private readonly IGenericRepository<User> _userRepository;

        public NotificationCreateCommandHandler(IGenericRepository<UserNotification> genericRepository,IEmailService emailService)
        {
            _genericRepository = genericRepository;
            _emailService = emailService;
        }
        public async Task<bool> Handle(NotificationCreateCommand request, CancellationToken cancellationToken)
        {
            var notification = new UserNotification(request.UserId, request.SubscriptionId,
                request.Title, request.Type);
            var htmlMessage = $@"<!DOCTYPE html>
            <html lang=""en"" style=""font-family: Arial, sans-serif; background:#f4f4f4;"">
            <head>
                <meta charset=""UTF-8"">
                <title>Subscription Renewal Reminder</title>
            </head>
            <body style=""margin:0; padding:0; background:#f4f4f4;"">

                <table role=""presentation"" style=""width:100%; border-collapse:collapse; padding:0; margin:0;"">
                    <tr>
                        <td align=""center"" style=""padding:20px 0;background:#004aad;color:#ffffff;font-size:28px;font-weight:bold;"">
                            Trackify Subscription Reminder
                        </td>
                    </tr>
                </table>

                <table role=""presentation"" style=""max-width:600px;margin:20px auto;background:#ffffff;border-radius:6px;overflow:hidden;"">
                    <tr>
                        <td style=""padding:20px 30px; font-size:16px; color:#333333;"">
                            <p>Hi <strong>{request.UserName}</strong>,</p>

                            <p>This is a friendly reminder that your subscription for 
                                <strong>{request.ServiceName}</strong> is renewing soon.</p>

                            <div style=""background:#f9fafc;border:1px solid #e1e4e8;padding:15px;margin:20px 0;border-radius:5px;"">
                                <p style=""margin:5px 0;""><strong>Renewal Date:</strong> {request.RenewalDate}</p>
                                <p style=""margin:5px 0;""><strong>Amount:</strong> {request.Cost}</p>
                            </div>

                            <p>If you wish to make changes, please manage your subscription from your Trackify dashboard.</p>

                            <p style=""margin-top:30px;"">Best regards,<br>
                            <strong>Trackify Team</strong></p>
                        </td>
                    </tr>

                    <tr>
                        <td style=""background:#004aad;text-align:center;padding:15px;color:#ffffff;font-size:12px;"">
                            © 2025 Trackify. All Rights Reserved.
                        </td>
                    </tr>
                </table>

            </body>
            </html>
            ";
            await _emailService.SendEmailAsync(request.UserEmail,request.Title,htmlMessage);
            await _genericRepository.AddAsync(notification,cancellationToken);

            return true;
        }
    }
}
