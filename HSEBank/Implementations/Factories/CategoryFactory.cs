using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Factories
{
    public class CategoryFactory : BaseFactory<CategoryId>, ICategoryFactory
    {
        private static readonly Dictionary<OperationType, int> TypeCounters = [];

        public Category Create(OperationType type, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Название категории не может быть пустым", nameof(name));
            }

            _ = TypeCounters.TryAdd(type, 1);

            string codePrefix = type == OperationType.Income ? "I" : "E";
            return new Category
            {
                Id = GenerateId(),
                Type = type,
                Name = name,
                Code = new CategoryCode($"{codePrefix}{TypeCounters[type]++:D3}")
            };
        }
    }
}
