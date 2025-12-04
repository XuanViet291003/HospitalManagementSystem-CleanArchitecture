using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Core.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendAccountActivationEmailAsync(string toEmail, string temporaryPassword);
    }
}
