using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Application.Abstracts;
using WebTicket.Domain.Entities;

namespace WebTicket.Infrastructure.HangFire
{
    public class HangfireJobScheduler : IBackGroundJobScheduler
    {
        public void ScheduleInProgressEvent(Event myEvent, TimeSpan delay)
        {
            //BackgroundJob.Schedule<IEventRepository>(
            //    x => x.InProgressEventAsync(myEvent),
            //    delay);
        }

    }

}
