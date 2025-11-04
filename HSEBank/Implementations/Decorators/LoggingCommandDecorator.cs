using HSEBank.Contracts;

namespace HSEBank.Implementations.Decorators
{
    public class LoggingCommandDecorator(ICommand decoratedCommand) : ICommand
    {
        private readonly ICommand _decoratedCommand = decoratedCommand;

        public void Execute()
        {
            Console.WriteLine($"[LOG] Выполнение команды начато в {DateTime.Now}");
            _decoratedCommand.Execute();
            Console.WriteLine($"[LOG] Выполнение команды завершено в {DateTime.Now}");
        }

        public void Undo()
        {
            Console.WriteLine($"[LOG] Отмена команды начата в {DateTime.Now}");
            _decoratedCommand.Undo();
            Console.WriteLine($"[LOG] Отмена команды завершена в {DateTime.Now}");
        }
    }
}
