using System;
using System.Threading.Tasks;
using Copious.Infrastructure.Interface.Services;

namespace Copious.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        private readonly IServiceProvider _serviceProvider;

        public EmailSender(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.FromResult(1);
        }
    }
}