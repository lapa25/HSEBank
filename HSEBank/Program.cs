using HSEBank.Contracts;
using HSEBank.Entities;
using HSEBank.Enums;
using HSEBank.Implementations.Commands;
using HSEBank.Implementations.Decorators;
using HSEBank.Implementations.Facades;
using HSEBank.Implementations.Factories;
using HSEBank.Implementations.Observers;
using HSEBank.Implementations.Repositories;
using HSEBank.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace HSEBank
{
    public class Program
    {
        public static void Main()
        {
            ServiceCollection services = new();

            _ = services.AddSingleton<IBankAccountFactory, BankAccountFactory>();
            _ = services.AddSingleton<ICategoryFactory, CategoryFactory>();
            _ = services.AddSingleton<IOperationFactory, OperationFactory>();

            _ = services.AddSingleton<IRepository<BankAccount, AccountId>, BankAccountRepository>();
            _ = services.AddSingleton<IRepository<Category, CategoryId>, CategoryRepository>();
            _ = services.AddSingleton<IRepository<Operation, OperationId>, OperationRepository>();

            _ = services.AddSingleton<IBankAccountFacade, BankAccountFacade>();
            _ = services.AddSingleton<ICategoryFacade, CategoryFacade>();
            _ = services.AddSingleton<IOperationFacade, OperationFacade>();
            _ = services.AddSingleton<IAnalyticsFacade, AnalyticsFacade>();

            _ = services.AddSingleton<OperationNotifier>();
            _ = services.AddSingleton<BalanceUpdateObserver>();

            ServiceProvider provider = services.BuildServiceProvider();

            DemoWork(provider);
        }

        public static void DemoWork(ServiceProvider provider)
        {
            Console.WriteLine("Демонстрация\n");

            IBankAccountFacade accountFacade = provider.GetService<IBankAccountFacade>()!;
            ICategoryFacade categoryFacade = provider.GetService<ICategoryFacade>()!;
            IOperationFacade operationFacade = provider.GetService<IOperationFacade>()!;
            IAnalyticsFacade analyticsFacade = provider.GetService<IAnalyticsFacade>()!;
            OperationNotifier notifier = provider.GetService<OperationNotifier>()!;
            BalanceUpdateObserver balanceObserver = provider.GetService<BalanceUpdateObserver>()!;

            // Подписка наблюдателей
            notifier.Subscribe(balanceObserver);

            Console.WriteLine("1.Фабрики и фасады");
            Console.WriteLine("----------------------\n");

            // Создание категорий
            CategoryId salaryCategoryId = categoryFacade.CreateCategory(OperationType.Income, "Зарплата");
            CategoryId foodCategoryId = categoryFacade.CreateCategory(OperationType.Expense, "Еда");
            CategoryId transportCategoryId = categoryFacade.CreateCategory(OperationType.Expense, "Транспорт");
            CategoryId entertainmentCategoryId = categoryFacade.CreateCategory(OperationType.Expense, "Развлечения");
            CategoryId bonusCategoryId = categoryFacade.CreateCategory(OperationType.Income, "Бонус");

            // Создание счетов
            AccountId mainAccountId = accountFacade.CreateAccount("Основной счет", new Money(5000));
            AccountId savingsAccountId = accountFacade.CreateAccount("Накопительный счет", new Money(10000));
            AccountId cardAccountId = accountFacade.CreateAccount("Карточный счет", new Money(2000));

            // Получение информации о созданных объектах
            Category? salaryCategory = categoryFacade.GetCategory(salaryCategoryId);
            BankAccount? mainAccount = accountFacade.GetAccount(mainAccountId);

            Console.WriteLine($"\nСоздана категория: {salaryCategory!.Name} (Код: {salaryCategory.Code})");
            Console.WriteLine($"Создан счет: {mainAccount!.Name} (Номер: {mainAccount.AccountNumber}, Баланс: {mainAccount.Balance})");

            Console.WriteLine("\n\n2. Типобезопасность");
            Console.WriteLine("----------------------\n");

            // Защита от отрицательных сумм
            Console.WriteLine("Проверка валидации PositiveDecimal:");
            try
            {
                PositiveDecimal negativeAmount = new(-100);
                Console.WriteLine("Ошибка: Отрицательная сумма была создана!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Защита сработала: {ex.Message}");
            }

            // Защита от невалидных номеров счетов
            Console.WriteLine("\nПроверка валидации AccountNumber:");
            try
            {
                AccountNumber invalidAccountNumber = new("INVALID123");
                Console.WriteLine("Ошибка: Невалидный номер счета был создан!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Защита сработала: {ex.Message}");
            }

            Console.WriteLine("\n\n3. Операции и наблюдатели");
            Console.WriteLine("----------------------\n");

            Console.WriteLine("Создание операций (наблюдатель автоматически обновляет балансы):");

            // Доходы
            OperationId salaryOperationId = operationFacade.CreateOperation(
                OperationType.Income,
                mainAccountId,
                new PositiveDecimal(50000),
                salaryCategoryId,
                "Зарплата за месяц");

            OperationId bonusOperationId = operationFacade.CreateOperation(
                OperationType.Income,
                savingsAccountId,
                new PositiveDecimal(5000),
                bonusCategoryId,
                "Годовой бонус");

            // Расходы
            OperationId foodOperationId = operationFacade.CreateOperation(
                OperationType.Expense,
                mainAccountId,
                new PositiveDecimal(1500),
                foodCategoryId,
                "Продукты на неделю");

            OperationId transportOperationId = operationFacade.CreateOperation(
                OperationType.Expense,
                mainAccountId,
                new PositiveDecimal(500),
                transportCategoryId,
                "Проездной на месяц");

            OperationId entertainmentOperationId = operationFacade.CreateOperation(
                OperationType.Expense,
                mainAccountId,
                new PositiveDecimal(2000),
                entertainmentCategoryId,
                "Кино и ресторан");


            // Проверка балансов после операций
            BankAccount? updatedMainAccount = accountFacade.GetAccount(mainAccountId);
            BankAccount? updatedSavingsAccount = accountFacade.GetAccount(savingsAccountId);

            Console.WriteLine($"\nБаланс основного счета после операций: {updatedMainAccount!.Balance}");
            Console.WriteLine($"Баланс накопительного счета: {updatedSavingsAccount!.Balance}");

            Console.WriteLine("\n\n4. Аналитика");
            Console.WriteLine("----------------------\n");


            Money balanceDiff = analyticsFacade.GetBalanceDifference(DateTime.Now.AddDays(-30), DateTime.Now);
            Dictionary<AccountId, Money> accountBalances = analyticsFacade.GetAccountBalances();
            Dictionary<string, Money> categorySummary = analyticsFacade.GetCategorySummaryWithNames(DateTime.Now.AddDays(-30), DateTime.Now);

            Console.WriteLine($"Разница доходов/расходов за период: {balanceDiff}");

            Console.WriteLine("\nБалансы всех счетов:");
            foreach (KeyValuePair<AccountId, Money> balance in accountBalances)
            {
                BankAccount? account = accountFacade.GetAccount(balance.Key);
                Console.WriteLine($"  {account!.Name}: {balance.Value}");
            }

            Console.WriteLine("\nСводка по категориям:");
            foreach (KeyValuePair<string, Money> summary in categorySummary)
            {
                Console.WriteLine($"  {summary.Key}: {summary.Value}");
            }

            Console.WriteLine("\n\n5. Команды и декораторы");
            Console.WriteLine("----------------------\n");

            // Простая команда
            Console.WriteLine("Простая команда создания счета:");
            ICommand createAccountCommand = new CreateAccountCommand(
                accountFacade, "Инвестиционный счет", new Money(50000));
            createAccountCommand.Execute();

            // Команда с таймингом
            Console.WriteLine("\nКоманда с таймингом:");
            ICommand timedCommand = new TimingCommandDecorator(
                new CreateAccountCommand(accountFacade, "Депозитный счет", new Money(30000)));
            timedCommand.Execute();

            // Команда с логированием и таймингом
            Console.WriteLine("\nКоманда с логированием и таймингом:");
            ICommand loggedAndTimedCommand = new TimingCommandDecorator(
                new LoggingCommandDecorator(
                    new CreateOperationCommand(
                        operationFacade,
                        OperationType.Income,
                        mainAccountId,
                        new PositiveDecimal(3000),
                        bonusCategoryId,
                        "Внеплановый бонус")));
            loggedAndTimedCommand.Execute();

            // Демонстрация отмены команды
            Console.WriteLine("\nДемонстрация отмены команды:");
            CreateAccountCommand undoCommand = new(accountFacade, "Временный счет", new Money(1000));
            undoCommand.Execute();
            Console.WriteLine("Отменяем создание счета...");
            undoCommand.Undo();

            Console.WriteLine("\n\n6.Поиск и фильтрация");
            Console.WriteLine("----------------------\n");

            AccountNumber a = new("ACC1000001");
            BankAccount? accountByNumber = accountFacade.GetAccountByNumber(a);
            Console.WriteLine($"Поиск счета по номеру ACC1000001: {accountByNumber?.Name}");

            Category? categoryByCode = categoryFacade.GetCategoryByCode(new CategoryCode("I001"));
            Console.WriteLine($"Поиск категории по коду I001: {categoryByCode?.Name}");

            // Фильтрация операций
            IEnumerable<Operation> mainAccountOperations = operationFacade.GetOperationsByAccount(mainAccountId);
            Console.WriteLine($"\nОперации по основному счету: {mainAccountOperations.Count()} операций");

            IEnumerable<Operation> foodOperations = operationFacade.GetOperationsByCategory(foodCategoryId);
            Console.WriteLine($"Операции по категории 'Еда': {foodOperations.Count()} операций");

            IEnumerable<Operation> incomeOperations = operationFacade.GetAllOperations()
                .Where(o => o.Type == OperationType.Income);
            Console.WriteLine($"Все доходные операции: {incomeOperations.Count()} операций");

            Console.WriteLine("\n\n7. Value objects");
            Console.WriteLine("----------------------\n");

            // Value objects арифметика
            Money money1 = new(1000);
            Money money2 = new(500);
            Money money3 = money1 + money2;
            Money money4 = money1 - money2;

            Console.WriteLine($"\nАрифметика Money: {money1} + {money2} = {money3}");
            Console.WriteLine($"Арифметика Money: {money1} - {money2} = {money4}");
            Console.WriteLine($"Сравнение Money: {money1} > {money2} => {money1 > money2}");

            // Работа с PositiveDecimal
            PositiveDecimal pos1 = new(100);
            PositiveDecimal pos2 = new(50);
            decimal pos3 = pos1 + pos2;

            Console.WriteLine($"\nАрифметика PositiveDecimal: {pos1} + {pos2} = {pos3}");
            Console.WriteLine($"Преобразование в Money: {pos1.ToMoney()}");

            Console.WriteLine("\n\n8. Работа с данными");
            Console.WriteLine("----------------------\n");

            // Получение всех данных
            IEnumerable<BankAccount> allAccounts = accountFacade.GetAllAccounts();
            IEnumerable<Category> allCategories = categoryFacade.GetAllCategories();
            IEnumerable<Operation> allOperations = operationFacade.GetAllOperations();

            Console.WriteLine($"Всего счетов: {allAccounts.Count()}");
            Console.WriteLine($"Всего категорий: {allCategories.Count()}");
            Console.WriteLine($"Всего операций: {allOperations.Count()}");

            // Детальная информация об операциях
            Console.WriteLine("\nПоследние 3 операции:");
            foreach (Operation? operation in allOperations.TakeLast(3))
            {
                BankAccount? operationAccount = accountFacade.GetAccount(operation.BankAccountId);
                Category? operationCategory = categoryFacade.GetCategory(operation.CategoryId);

                Console.WriteLine($"  {operation.Date:dd.MM.yyyy} | " +
                                $"{operationCategory?.Name} | " +
                                $"{operation.Amount} | " +
                                $"{operation.Description} | " +
                                $"{operationAccount?.Name}");
            }

            Console.WriteLine("\n\n9. Обновление данных");
            Console.WriteLine("----------------------\n");

            // Обновление счета
            BankAccount? accountToUpdate = accountFacade.GetAccount(mainAccountId);
            if (accountToUpdate != null)
            {
                string oldName = accountToUpdate.Name;
                accountToUpdate.Name = "Основной расчетный счет";
                accountFacade.UpdateAccount(accountToUpdate);
                Console.WriteLine($"Счет переименован: '{oldName}' -> '{accountToUpdate.Name}'");
            }

            // Обновление категории
            Category? categoryToUpdate = categoryFacade.GetCategory(foodCategoryId);
            if (categoryToUpdate != null)
            {
                string oldName = categoryToUpdate.Name;
                categoryToUpdate.Name = "Продукты питания";
                // Для обновления категории нужно получить репозиторий или добавить метод в фасад
                Console.WriteLine($"Категория переименована: '{oldName}' -> '{categoryToUpdate.Name}'");
            }

            Console.WriteLine("\n\n10. Удаление данных");
            Console.WriteLine("----------------------\n");

            // Временный счет для демонстрации удаления
            AccountId tempAccountId = accountFacade.CreateAccount("Временный счет для удаления", new Money(100));
            Console.WriteLine($"Создан временный счет: {tempAccountId}");

            // Удаление счета
            accountFacade.DeleteAccount(tempAccountId);
            Console.WriteLine($"Счет {tempAccountId} удален");

            // Проверка, что счет действительно удален
            BankAccount? deletedAccount = accountFacade.GetAccount(tempAccountId);
            Console.WriteLine($"Проверка удаления: счет {(deletedAccount == null ? "не найден" : "найден (ошибка)")}");

            Console.WriteLine("\n\nКонец демонстрации");
            Console.WriteLine("Итоговое состояние системы:");
            Console.WriteLine($"- Счетов: {accountFacade.GetAllAccounts().Count()}");
            Console.WriteLine($"- Категорий: {categoryFacade.GetAllCategories().Count()}");
            Console.WriteLine($"- Операций: {operationFacade.GetAllOperations().Count()}");
            Console.WriteLine($"- Общий баланс: {analyticsFacade.GetAccountBalances().Values.Sum(b => b.Value)}");
        }
    }
}