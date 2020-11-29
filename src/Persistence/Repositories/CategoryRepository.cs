using Application.Data.Interfaces;
using Domain.Entities;


namespace Persistence.Repositories
{
    public class CategoryRepository : EfCoreRepositoryBase<DbContextBase, Category, int>, ICategoryRepository
    {
        public CategoryRepository(DbContextBase context) : base(context)
        {
        }
    }
}