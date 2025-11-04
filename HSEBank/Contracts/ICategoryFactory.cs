using HSEBank.Entities;
using HSEBank.Enums;

namespace HSEBank.Contracts
{
    public interface ICategoryFactory
    {
        Category Create(OperationType type, string name);
    }
}
