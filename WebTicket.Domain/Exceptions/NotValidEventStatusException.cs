using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Exceptions
{
    public class NotValidEventStatusException(string status): Exception($"{status} required to do this action");
}
