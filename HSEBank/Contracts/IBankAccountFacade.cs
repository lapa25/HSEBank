using HSEBank.Entities;
using HSEBank.ValueObjects;

namespace HSEBank.Contracts
{
    public interface IBankAccountFacade
    {
        AccountId CreateAccount(string name, Money initialBalance);
        void DeleteAccount(AccountId id);
        BankAccount? GetAccount(AccountId id);
        IEnumerable<BankAccount> GetAllAccounts();
        void UpdateAccount(BankAccount account);
        BankAccount? GetAccountByNumber(AccountNumber accountNumber);
    }
}
