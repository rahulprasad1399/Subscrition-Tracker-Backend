using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailTestController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendTestEmail(string email)
        {
            await _emailService.SendEmailAsync(
                email,
                "Test Email from Brevo",
                "<h2>Welcome!</h2><p>Your Brevo Email setup works! 🚀</p>"
            );

            return Ok("Email Sent Successfully ✔");
        }
    }
}
