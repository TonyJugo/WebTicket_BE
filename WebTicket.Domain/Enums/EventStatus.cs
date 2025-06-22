using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Domain.Enums
{
    public enum EventStatus
    {
        Private = 1,
        Published = 2,
        Cancelled = 3,
        InProgress = 4,
        Completed = 5,
        SoldOut = 6
    }
}
