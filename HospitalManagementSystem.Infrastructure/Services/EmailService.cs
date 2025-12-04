using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalManagementSystem.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;

        public EmailService(ILoggerFactory  loggerFactory )
        {
            _logger = loggerFactory.CreateLogger("EmailService");
        }

        public Task SendAccountActivationEmailAsync(string toEmail ,string temporaryPassword)
        {
            _logger.LogInformation("SIMULATING SENDING EMAIL");
            _logger.LogInformation($"To: {toEmail}");
            _logger.LogInformation("Subject: Kích hoạt tài khoản của bạn");
            _logger.LogInformation("Body:");
            _logger.LogInformation($"Chào bạn, tài khoản của bạn đã được tạo.");
            _logger.LogInformation($"Mật khẩu tạm thời của bạn là: {temporaryPassword}");
            _logger.LogInformation("Vui lòng đăng nhập và đổi mật khẩu ngay lập tức.");
            _logger.LogInformation("Cảm ơn bạn đã tin tưởng dịch vụ của chúng tôi");

            return Task.CompletedTask;
        }

    }
}
