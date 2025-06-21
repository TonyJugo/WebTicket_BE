using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Exceptions
{
    public class UserNotFoundException(string message) : Exception(message);
}
