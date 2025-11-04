using HSEBank.Contracts;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Commands
{
    public class CreateAccountCommand(IBankAccountFacade facade, string name, Money balance) : ICommand
    {
        private readonly IBankAccountFacade _facade = facade;
        private readonly string _name = name;
        private readonly Money _balance = balance;
        private AccountId _createdAccountId = new(0);

        public void Execute()
        {
            _createdAccountId = _facade.CreateAccount(_name, _balance);
            Entities.BankAccount? account = _facade.GetAccount(_createdAccountId);
            Console.WriteLine($"Создан счет: {_name} (№{account?.AccountNumber}) с балансом {_balance}. ID: {_createdAccountId}");
        }

        public void Undo()
        {
            if (!_createdAccountId.Equals(new AccountId(0)))
            {
                _facade.DeleteAccount(_createdAccountId);
                Console.WriteLine($"Отменено создание счета: {_name}");
            }
        }
    }
}
