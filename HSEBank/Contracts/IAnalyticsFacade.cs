using HSEBank.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSEBank.Contracts
{
    public interface IAnalyticsFacade
    {
        Money GetBalanceDifference(DateTime start, DateTime end);
        Dictionary<CategoryId, Money> GetCategorySummary(DateTime start, DateTime end);
        Dictionary<AccountId, Money> GetAccountBalances();
        Dictionary<string, Money> GetCategorySummaryWithNames(DateTime start, DateTime end);
    }
}
