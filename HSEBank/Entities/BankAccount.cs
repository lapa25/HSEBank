using HSEBank.ValueObjects;

namespace HSEBank.Entities
{
    public class BankAccount
    {
        public AccountId Id { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public Money Balance { get; set; }
        public AccountNumber AccountNumber { get; set; }
    }
}
