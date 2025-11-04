using HSEBank.Contracts;

namespace HSEBank.Implementations.Decorators
{
    public class TimingCommandDecorator(ICommand decoratedCommand) : ICommand
    {
        private readonly ICommand _decoratedCommand = decoratedCommand;

        public void Execute()
        {
            DateTime startTime = DateTime.Now;
            _decoratedCommand.Execute();
            DateTime endTime = DateTime.Now;
            Console.WriteLine($"Время выполнения: {(endTime - startTime).TotalMilliseconds} мс");
        }

        public void Undo()
        {
            DateTime startTime = DateTime.Now;
            _decoratedCommand.Undo();
            DateTime endTime = DateTime.Now;
            Console.WriteLine($"Время отмены: {(endTime - startTime).TotalMilliseconds} мс");
        }
    }
}
