using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTicket.Application.Abstracts
{
    public interface IEventJobScheduler
    {
        Task ScheduleUpdateStatusJobAsync(string eventId, DateTime? startDate);
        Task DeleteStatusJobAsync(string eventId);
    }
}
