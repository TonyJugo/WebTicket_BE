using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Exceptions
{
    public class UniversityNameAlreadyExistsException(string name) : Exception($"University with name: {name} already exists");
}
