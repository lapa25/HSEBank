using HSEBank.Contracts;
using HSEBank.Entities;

namespace HSEBank.Implementations.Observers
{
    public class OperationNotifier
    {
        private readonly List<IOperationObserver> _observers = [];

        public void Subscribe(IOperationObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unsubscribe(IOperationObserver observer)
        {
            _ = _observers.Remove(observer);
        }

        public void Notify(Operation operation)
        {
            foreach (IOperationObserver observer in _observers)
            {
                observer.OnOperationCreated(operation);
            }
        }
    }
}
