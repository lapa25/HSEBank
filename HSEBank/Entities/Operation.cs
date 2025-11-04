using HSEBank.ValueObjects;
using HSEBank.Enums;

namespace HSEBank.Entities
{
    public class Operation
    {
        public OperationId Id { get; set; } = null!;
        public OperationType Type { get; set; }
        public AccountId BankAccountId { get; set; } = null!;
        public PositiveDecimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public CategoryId CategoryId { get; set; } = null!;
        public TransactionId TransactionId { get; set; }
    }
}
