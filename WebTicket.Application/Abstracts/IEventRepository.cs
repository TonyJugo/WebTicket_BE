using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Domain.Constants;
using WebTicket.Domain.Entities;

namespace WebTicket.Application.Abstracts
{
    public interface IEventRepository
    {
        Task<List<Event>> GetAllEventsAsync();

        Task<List<Event>> GetAllCompletedEvent();
        Task<List<Event>> GetAllPrivateEvent();
        Task<List<Event>> GetAllCancelledEvent();
        Task<List<Event>> GetAllPublishedEvent();

        Task AddEventAsync(Event myEvent);

        Task<Event?> GetEventByIdAsync(string id);

        Task UpdateEventAsync(Event myEvent);


        Task<string> GetLastId();

        Task<Event> GetEventByNameAsync(string name);


    }
}
