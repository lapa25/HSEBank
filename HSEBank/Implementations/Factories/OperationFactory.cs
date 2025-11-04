using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Factories
{
    public class OperationFactory : BaseFactory<OperationId>, IOperationFactory
    {
        private static long _transactionCounter = 1;

        public Operation Create(OperationType type, AccountId accountId, PositiveDecimal amount, CategoryId categoryId, string description = "")
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            return new Operation
            {
                Id = GenerateId(),
                Type = type,
                BankAccountId = accountId,
                Amount = amount,
                CategoryId = categoryId,
                Description = description,
                Date = DateTime.Now,
                TransactionId = new TransactionId($"TXN{timestamp}_{_transactionCounter++}")
            };
        }
    }
}
