using HSEBank.Entities;
using HSEBank.ValueObjects;

namespace HSEBank.Implementations.Repositories
{
    public class BankAccountRepository : BaseRepository<BankAccount, AccountId>
    {
        protected override AccountId GetId(BankAccount entity)
        {
            return entity.Id;
        }
    }
}
