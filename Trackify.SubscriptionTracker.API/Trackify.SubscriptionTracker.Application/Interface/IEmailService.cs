using System.Threading.Tasks;
namespace Trackify.SubscriptionTracker.Application.Interface
{
    public interface IEmailService
    {
        Task<int> SendEmailAsync(string to, string subject, string htmlContent);
    }
}
