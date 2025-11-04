using HSEBank.Entities;
using HSEBank.ValueObjects;

namespace HSEBank.Contracts
{
    public interface IBankAccountFactory
    {
        BankAccount Create(string name, Money initialBalance);
    }
}
