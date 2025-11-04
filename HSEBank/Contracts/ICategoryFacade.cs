using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSEBank.Contracts
{
    public interface ICategoryFacade
    {
        CategoryId CreateCategory(OperationType type, string name);
        void DeleteCategory(CategoryId id);
        Category? GetCategory(CategoryId id);
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Category> GetCategoriesByType(OperationType type);
        Category? GetCategoryByCode(CategoryCode code);
    }
}
