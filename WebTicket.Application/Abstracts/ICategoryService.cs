using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Domain.Entities;
using WebTicket.Domain.Exceptions;
using WebTicket.Domain.Requests;

namespace WebTicket.Application.Abstracts
{
    public interface ICategoryService
    {

        Task<List<Category>> GetAllCategories();

        Task<List<Category>> GetAllDisabledCategories();

        Task AddCategoryAsync(CategoryRequest categoryRequest);

        Task<Category?> GetCategoryByIdAsync(string id);

        Task UpdateCategoryAsync(string id, CategoryUpdateRequest categoryRequest);

        Task DeleteCategoryAsync(string id);
    }

}

