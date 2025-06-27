using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Exceptions
{
    public class ObjPropertyAlreadyExists(string obj) : Exception($"{obj} already exists");

}
