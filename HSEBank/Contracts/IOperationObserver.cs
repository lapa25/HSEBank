using HSEBank.Entities;

namespace HSEBank.Contracts
{
    public interface IOperationObserver
    {
        void OnOperationCreated(Operation operation);
    }
}
