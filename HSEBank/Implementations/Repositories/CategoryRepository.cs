using HSEBank.Entities;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Repositories
{
    public class CategoryRepository : BaseRepository<Category, CategoryId>
    {
        protected override CategoryId GetId(Category entity)
        {
            return entity.Id;
        }
    }
}
