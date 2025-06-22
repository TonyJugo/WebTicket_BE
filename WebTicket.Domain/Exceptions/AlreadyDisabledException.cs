using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Exceptions
{
    public class AlreadyDisabledException(string obj) : Exception($"{obj} is already disabled");
}
