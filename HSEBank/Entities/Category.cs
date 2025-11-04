using HSEBank.ValueObjects;
using HSEBank.Enums;

namespace HSEBank.Entities
{
    public class Category
    {
        public CategoryId Id { get; set; } = null!;
        public OperationType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public CategoryCode Code { get; set; }
    }
}
