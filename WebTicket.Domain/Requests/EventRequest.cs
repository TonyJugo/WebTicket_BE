using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Requests
{
    public record EventRequest(string Name, string Description, DateTime Date_Start, DateTime Date_End, int Price, string CategoryName);
}
