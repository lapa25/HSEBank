using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Observers
{
    public class BalanceUpdateObserver(IRepository<BankAccount, AccountId> accountRepository) : IOperationObserver
    {
        private readonly IRepository<BankAccount, AccountId> _accountRepository = accountRepository;

        public void OnOperationCreated(Operation operation)
        {
            BankAccount? account = _accountRepository.Get(operation.BankAccountId);
            if (account != null)
            {
                Money adjustment = operation.Type == OperationType.Income ?
                   operation.Amount.ToMoney() :
                   operation.Amount.ToNegativeMoney();

                account.Balance += adjustment;
                _accountRepository.Update(account);
                Console.WriteLine($"Баланс счета {account.Name} ({account.AccountNumber}) обновлен: {account.Balance}");
            }
        }
    }
}
