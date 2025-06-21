using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Exceptions
{
    public class ObjectNotFoundException(string objectName) : Exception($"{objectName} not found");
}
