using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Facades
{
    public class CategoryFacade(ICategoryFactory factory, IRepository<Category, CategoryId> repository) : ICategoryFacade
    {
        private readonly ICategoryFactory _factory = factory;
        private readonly IRepository<Category, CategoryId> _repository = repository;

        public CategoryId CreateCategory(OperationType type, string name)
        {
            Category category = _factory.Create(type, name);
            _repository.Add(category);
            return category.Id;
        }

        public void DeleteCategory(CategoryId id)
        {
            _repository.Remove(id);
        }

        public Category? GetCategory(CategoryId id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Category> GetCategoriesByType(OperationType type)
        {
            return _repository.GetAll().Where(c => c.Type == type);
        }

        public Category? GetCategoryByCode(CategoryCode code)
        {
            return _repository.GetAll().FirstOrDefault(c => c.Code.Equals(code));
        }
    }
}
