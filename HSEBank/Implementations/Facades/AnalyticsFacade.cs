using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Facades
{
    public class AnalyticsFacade(IRepository<Operation, OperationId> operationRepository,
        IRepository<BankAccount, AccountId> accountRepository,
        IRepository<Category, CategoryId> categoryRepository) : IAnalyticsFacade
    {
        private readonly IRepository<Operation, OperationId> _operationRepository = operationRepository;
        private readonly IRepository<BankAccount, AccountId> _accountRepository = accountRepository;
        private readonly IRepository<Category, CategoryId> _categoryRepository = categoryRepository;

        public Money GetBalanceDifference(DateTime start, DateTime end)
        {
            IEnumerable<Operation> operations = _operationRepository.GetAll()
                .Where(o => o.Date >= start && o.Date <= end);

            decimal income = operations.Where(o => o.Type == OperationType.Income).Sum(o => (decimal)o.Amount);
            decimal expense = operations.Where(o => o.Type == OperationType.Expense).Sum(o => (decimal)o.Amount);

            return new Money(income - expense);
        }

        public Dictionary<CategoryId, Money> GetCategorySummary(DateTime start, DateTime end)
        {
            Dictionary<CategoryId, Money> summary = _operationRepository.GetAll()
                .Where(o => o.Date >= start && o.Date <= end)
                .GroupBy(o => o.CategoryId)
                .ToDictionary(g => g.Key, g =>
                {
                    decimal total = g.Sum(o => o.Type == OperationType.Income ? (decimal)o.Amount : -(decimal)o.Amount);
                    return new Money(total);
                });

            return summary;
        }

        public Dictionary<AccountId, Money> GetAccountBalances()
        {
            return _accountRepository.GetAll()
                .ToDictionary(a => a.Id, a => a.Balance);
        }

        public Dictionary<string, Money> GetCategorySummaryWithNames(DateTime start, DateTime end)
        {
            Dictionary<CategoryId, Money> summary = GetCategorySummary(start, end);
            Dictionary<string, Money> result = [];

            foreach (KeyValuePair<CategoryId, Money> item in summary)
            {
                Category? category = _categoryRepository.Get(item.Key);
                string categoryName = category?.Name ?? $"Категория {item.Key}";
                result[categoryName] = item.Value;
            }

            return result;
        }
    }
}
