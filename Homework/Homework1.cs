using Task.Features;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework1 : ITask
    {
        public void Start()
        {
            h_t_04_09_2024.Task2.IBankAccount? account = null;

            Dictionary<string, Action>? accountCreator = null;

            accountCreator = new Dictionary<string, Action>{
                    { "Deposit Account", () => account = CreateDepositAccount() },
                    { "Current Account", () => account = CreateCurrentAccount() },
                };

            Dictionary<string, Action>? accountActions = null;

            accountActions = new Dictionary<string, Action>
                {
                    { "Display Balance", () => account?.DisplayBalance() },
                    {
                        "Deposit", () =>
                        {
                            Console.Write("Enter deposit amount: ");

                            if (double.TryParse(Console.ReadLine(), out double depositAmount))
                                account?.Deposit(depositAmount);
                            else
                                Console.WriteLine("Invalid amount.");
                        }},
                    {
                        "Withdraw", () =>
                        {
                            Console.Write("Enter withdraw amount: ");

                            if (double.TryParse(Console.ReadLine(), out double withdrawAmount))
                                account?.Withdraw(withdrawAmount);
                            else
                                Console.WriteLine("Invalid amount.");
                        }},
                };

            while (true)
            {
                StringFeatures.DisplayDashes();
                try
                {
                    if (!TaskHelper.MakeTasksExecutor(accountCreator))
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                while (true)
                {
                    StringFeatures.DisplayDashes();
                    try
                    {
                        if (!TaskHelper.MakeTasksExecutor(accountActions))
                            break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        static h_t_04_09_2024.Task2.IBankAccount CreateDepositAccount()
        {
            Console.Write("Enter account number: ");
            var number = Console.ReadLine();

            Console.Write("Enter account owner: ");
            var owner = Console.ReadLine();

            Console.Write("Enter initial balance: ");
            var balance = double.Parse(Console.ReadLine()!);

            return new h_t_04_09_2024.Task2.DepositAccount(number!, owner!, balance);
        }

        static h_t_04_09_2024.Task2.IBankAccount CreateCurrentAccount()
        {
            Console.Write("Enter account number: ");
            var number = Console.ReadLine();

            Console.Write("Enter account owner: ");
            var owner = Console.ReadLine();

            Console.Write("Enter initial balance: ");
            var balance = double.Parse(Console.ReadLine()!);

            return new h_t_04_09_2024.Task2.CurrentAccount(number!, owner!, balance);
        }
    }
}

namespace Task.Homework.h_t_04_09_2024
{
    namespace Task1
    {
        abstract class BankAccount(string accountNumber, string owner, double initialBalance)
        {
            public string AccountNumber { get; set; } = accountNumber;
            public string Owner { get; set; } = owner;
            public double Balance { get; protected set; } = initialBalance;

            public abstract void DisplayBalance();
            public abstract void Deposit(double amount);
            public abstract void Withdraw(double amount);
        }

        class DepositAccount(string accountNumber, string owner, double initialBalance) : BankAccount(accountNumber, owner, initialBalance)
        {
            public override void DisplayBalance()
                => Console.WriteLine($"Deposit Account Balance: {Balance}");

            public override void Deposit(double amount)
            {
                if (amount < 1)
                {
                    Console.WriteLine("Deposit amount should be greater than 0");
                    return;
                }
                Balance += amount;
                Console.WriteLine($"Deposited {amount} into Deposit Account.");
            }

            public override void Withdraw(double amount)
                => Console.WriteLine("Withdrawals are not allowed from Deposit Accounts.");
        }

        class CurrentAccount(string accountNumber, string owner, double initialBalance) : BankAccount(accountNumber, owner, initialBalance)
        {
            public override void DisplayBalance()
                => Console.WriteLine($"Current Account Balance: {Balance}");

            public override void Deposit(double amount)
            {
                if (amount < 1)
                {
                    Console.WriteLine("Deposit amount should be greater than 0");
                    return;
                }
                Balance += amount;
                Console.WriteLine($"Deposited {amount} into Current Account.");
            }

            public override void Withdraw(double amount)
            {
                if (amount < 1)
                {
                    Console.WriteLine("Deposit amount should be greater than 0");
                    return;
                }
                if (amount > Balance)
                {
                    Console.WriteLine("Insufficient funds.");
                    return;
                }
                Balance -= amount;
                Console.WriteLine($"Withdrew {amount} from Current Account.");
            }
        }
    }

    namespace Task2
    {
        interface IBankAccount
        {
            string AccountNumber { get; }
            string Owner { get; }
            double Balance { get; }
            void DisplayBalance();
            void Deposit(double amount);
            void Withdraw(double amount);
        }

        class DepositAccount(string accountNumber, string owner, double initialBalance) : IBankAccount
        {
            public string AccountNumber { get; private set; } = accountNumber;
            public string Owner { get; private set; } = owner;
            public double Balance { get; private set; } = initialBalance;

            public void DisplayBalance()
                => Console.WriteLine($"Deposit Account Balance: {Balance}");

            public void Deposit(double amount)
            {
                if (amount < 1)
                {
                    Console.WriteLine("Deposit amount should be greater than 0");
                    return;
                }
                Balance += amount;
                Console.WriteLine($"Deposited {amount} into Deposit Account.");
            }

            public void Withdraw(double amount)
                => Console.WriteLine("Withdrawals are not allowed from Deposit Accounts.");
        }

        class CurrentAccount(string accountNumber, string owner, double initialBalance) : IBankAccount
        {
            public string AccountNumber { get; private set; } = accountNumber;
            public string Owner { get; private set; } = owner;
            public double Balance { get; private set; } = initialBalance;

            public void DisplayBalance()
                => Console.WriteLine($"Current Account Balance: {Balance}");

            public void Deposit(double amount)
            {
                if (amount < 1)
                {
                    Console.WriteLine("Deposit amount should be greater than 0");
                    return;
                }
                Balance += amount;
                Console.WriteLine($"Deposited {amount} into Current Account.");
            }

            public void Withdraw(double amount)
            {
                if (amount < 1)
                {
                    Console.WriteLine("Withdraw amount should be greater than 0");
                    return;
                }
                if (amount > Balance)
                {
                    Console.WriteLine("Insufficient funds.");
                    return;
                }
                Balance -= amount;
                Console.WriteLine($"Withdrew {amount} from Current Account.");
            }
        }
    }
}