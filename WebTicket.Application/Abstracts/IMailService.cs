using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Application.Contracts;

namespace WebTicket.Application.Abstracts
{
    public interface IMailService
    {
        Task SendEmailAsync(SendEmailRequest sendEmailRequest);
    }
}
