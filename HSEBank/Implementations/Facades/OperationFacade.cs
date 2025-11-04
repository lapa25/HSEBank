using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;
using HSEBank.Implementations.Observers;

namespace HSEBank.Implementations.Facades
{
    public class OperationFacade(IOperationFactory factory,
        IRepository<Operation, OperationId> operationRepository,
        IRepository<BankAccount, AccountId> accountRepository,
        OperationNotifier notifier) : IOperationFacade
    {
        private readonly IOperationFactory _factory = factory;
        private readonly IRepository<Operation, OperationId> _operationRepository = operationRepository;
        private readonly IRepository<BankAccount, AccountId> _accountRepository = accountRepository;
        private readonly OperationNotifier _notifier = notifier;

        public OperationId CreateOperation(OperationType type, AccountId accountId, PositiveDecimal amount, CategoryId categoryId, string description = "")
        {
            Operation operation = _factory.Create(type, accountId, amount, categoryId, description);
            _operationRepository.Add(operation);

            _notifier.Notify(operation);

            return operation.Id;
        }

        public void DeleteOperation(OperationId id)
        {
            _operationRepository.Remove(id);
        }

        public Operation? GetOperation(OperationId id)
        {
            return _operationRepository.Get(id);
        }

        public IEnumerable<Operation> GetAllOperations()
        {
            return _operationRepository.GetAll();
        }

        public IEnumerable<Operation> GetOperationsByAccount(AccountId accountId)
        {
            return _operationRepository.GetAll().Where(o => o.BankAccountId.Equals(accountId));
        }

        public IEnumerable<Operation> GetOperationsByCategory(CategoryId categoryId)
        {
            return _operationRepository.GetAll().Where(o => o.CategoryId.Equals(categoryId));
        }

        public Operation? GetOperationByTransactionId(TransactionId transactionId)
        {
            return _operationRepository.GetAll().FirstOrDefault(o => o.TransactionId.Equals(transactionId));
        }
    }
}
