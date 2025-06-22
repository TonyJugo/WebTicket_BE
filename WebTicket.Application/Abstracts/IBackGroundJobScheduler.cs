using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Domain.Entities;

namespace WebTicket.Application.Abstracts
{
    public interface IBackGroundJobScheduler
    {
        void ScheduleInProgressEvent(Event myEvent, TimeSpan delay);
    }
}
