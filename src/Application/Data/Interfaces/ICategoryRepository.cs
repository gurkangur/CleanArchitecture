using Domain.Repositories;
using Domain.Entities;

namespace Application.Data.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, int>
    {
    }
}