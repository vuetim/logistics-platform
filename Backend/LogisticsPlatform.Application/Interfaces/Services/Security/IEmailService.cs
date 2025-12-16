using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsPlatform.Application.Interfaces.Services.Security
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body, string? attachmentName = null, byte[]? attachmentBytes = null);

    }

}
