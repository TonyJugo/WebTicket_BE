using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Requests
{
    //tự động tạo Getter, constructor, các property là init-only
    public record SendEmailRequest(string Recipient, string Subject, string Body, bool isHtml);
}
