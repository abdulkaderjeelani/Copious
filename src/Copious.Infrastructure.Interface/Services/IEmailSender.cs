using System.Threading.Tasks;

namespace Copious.Infrastructure.Interface.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}