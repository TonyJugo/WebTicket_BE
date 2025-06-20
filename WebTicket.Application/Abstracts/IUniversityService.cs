using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Domain.Entities;
using WebTicket.Domain.Requests;

namespace WebTicket.Application.Abstracts
{
    public interface IUniversityService
    {
        Task<List<string>> GetAllUniversityNames();
        Task<List<University>> GetAllUniversity();
        Task AddUniversity(UniversityRequest university);
        Task UpdateUniversityById(string id, UniversityRequest request);
        Task DeleteUniversityById(string id);
        Task<University?> GetUniversityById(string id);

    }
}
