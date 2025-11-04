using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSEBank.Contracts
{
    public interface IOperationFacade
    {
        OperationId CreateOperation(OperationType type, AccountId accountId, PositiveDecimal amount, CategoryId categoryId, string description = "");
        void DeleteOperation(OperationId id);
        Operation? GetOperation(OperationId id);
        IEnumerable<Operation> GetAllOperations();
        IEnumerable<Operation> GetOperationsByAccount(AccountId accountId);
        IEnumerable<Operation> GetOperationsByCategory(CategoryId categoryId);
        Operation? GetOperationByTransactionId(TransactionId transactionId);
    }
}
