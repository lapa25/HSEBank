using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;

namespace HSEBank.Contracts
{
    public interface IOperationFactory
    {
        Operation Create(OperationType type, AccountId accountId, PositiveDecimal amount, CategoryId categoryId, string description = "");
    }
}
