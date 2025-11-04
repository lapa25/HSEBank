using HSEBank.Contracts;
using HSEBank.Enums;
using HSEBank.ValueObjects;
namespace HSEBank.Implementations.Commands
{
    public class CreateOperationCommand(IOperationFacade facade, OperationType type, AccountId accountId,
        PositiveDecimal amount, CategoryId categoryId, string description = "") : ICommand
    {
        private readonly IOperationFacade _facade = facade;
        private readonly OperationType _type = type;
        private readonly AccountId _accountId = accountId;
        private readonly PositiveDecimal _amount = amount;
        private readonly CategoryId _categoryId = categoryId;
        private readonly string _description = description;
        private OperationId _createdOperationId = new(0);

        public void Execute()
        {
            _createdOperationId = _facade.CreateOperation(_type, _accountId, _amount, _categoryId, _description);
            Entities.Operation? operation = _facade.GetOperation(_createdOperationId);
            Console.WriteLine($"Создана операция: {_type} на сумму {_amount}. Транзакция: {operation?.TransactionId}");
        }

        public void Undo()
        {
            if (!_createdOperationId.Equals(new OperationId(0)))
            {
                _facade.DeleteOperation(_createdOperationId);
                Console.WriteLine($"Отменено создание операции");
            }
        }
    }
}
