using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Factories
{
    public class BankAccountFactory : BaseFactory<AccountId>, IBankAccountFactory
    {
        private static int _nextAccountNumber = 1000001;

        public BankAccount Create(string name, Money initialBalance)
        {
            return string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException("Название счета не может быть пустым", nameof(name))
                : new BankAccount
            {
                Id = GenerateId(),
                Name = name,
                Balance = initialBalance,
                AccountNumber = new AccountNumber($"ACC{_nextAccountNumber++}")
            };
        }
    }
}
