using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Facades
{
    public class BankAccountFacade(IBankAccountFactory factory, IRepository<BankAccount, AccountId> repository) : IBankAccountFacade
    {
        private readonly IBankAccountFactory _factory = factory;
        private readonly IRepository<BankAccount, AccountId> _repository = repository;

        public AccountId CreateAccount(string name, Money initialBalance)
        {
            BankAccount account = _factory.Create(name, initialBalance);
            _repository.Add(account);
            return account.Id;
        }

        public void DeleteAccount(AccountId id)
        {
            _repository.Remove(id);
        }

        public BankAccount? GetAccount(AccountId id)
        {
            return _repository.Get(id);
        }

        public IEnumerable<BankAccount> GetAllAccounts()
        {
            return _repository.GetAll();
        }

        public void UpdateAccount(BankAccount account)
        {
            _repository.Update(account);
        }

        public BankAccount? GetAccountByNumber(AccountNumber accountNumber)
        {
            return _repository.GetAll().FirstOrDefault(a => a.AccountNumber.Equals(accountNumber));
        }
    }
}
