using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Task.Features;
using static Task.TasksWorkers;

namespace Task
{
    namespace Features
    {
        public static partial class StringFeatures
        {
            [GeneratedRegex("([A-Z])", RegexOptions.Compiled)]
            private static partial Regex CamelCaseRule();

            public static void DisplayDashes()
                => Console.WriteLine("<------------------------------------------------->");

            public static string CamelCaseToSpace(this string input)
                => CamelCaseRule().Replace(input, " $1").Trim();

            public static string HashPassword(this string password)
                => string.Concat(SHA256.HashData(Encoding.UTF8.GetBytes(password)).Select(b => b.ToString("x2")));
        }

        public static class TaskHelper
        {
            public static void MakeTasksExecutor(params Action[] actions)
            {
                Console.WriteLine($"Enter number of task(1-{actions.Length})");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= actions.Length)
                    actions[choice - 1].Invoke();
                else
                    Console.WriteLine("Incorrect number");
            }
        }
    }
    namespace l_t1_03_09_2024
    {
        public abstract class Animal
        {
            public abstract void Sound();
            public abstract void Walk();
        }

        public class Snake(string voice = "shhh", int speed = 2) : Animal
        {
            public string Voice { get; } = voice;
            public int Speed { get; } = speed;

            public override void Sound() => Console.WriteLine($"{GetType().Name}: {Voice}");

            public override void Walk() => Console.WriteLine($"{GetType().Name}: {Speed}");
        }

        public class Dog(string voice = "guff", int speed = 5) : Animal
        {
            public string Voice { get; } = voice;
            public int Speed { get; } = speed;

            public override void Sound() => Console.WriteLine($"{GetType().Name}: {Voice}");

            public override void Walk() => Console.WriteLine($"{GetType().Name}: {Speed}");
        }

        public class Cat(string voice = "meew", int speed = 6) : Animal
        {
            public string Voice { get; } = voice;
            public int Speed { get; } = speed;

            public override void Sound() => Console.WriteLine($"{GetType().Name}: {Voice}");

            public override void Walk() => Console.WriteLine($"{GetType().Name}: {Speed}");
        }
    }
    namespace l_t2_03_09_2024
    {
        public interface IIdentify
        {
            public string SpecificCode { get; }
        }

        public class PrivateBankIdentify : IIdentify
        {
            public string SpecificCode { get; } = Path.GetRandomFileName();
        }

        public abstract class BankAccount(IIdentify identify, double balance, double limit = double.MaxValue)
        {
            public string BankNumber { get; } = Guid.NewGuid().ToString();
            public IIdentify Identify { get; } = identify;
            public double Balance { get; set; } = balance;
            protected double Limit { get; set; } = limit;

            public void Deposit(double percentage)
            {
                Balance += Balance * percentage / 100;
            }

            public void Current(double newLimit)
            {
                Limit = newLimit;
            }

            public override string ToString() =>
                $"{nameof(Balance)}: {Balance}\n{nameof(Identify)}: {Identify.SpecificCode}\n{nameof(BankNumber).CamelCaseToSpace()}: {BankNumber}\n{nameof(Limit)}: {Limit}";
        }

        public class PrivateBankAccount(PrivateBankIdentify person, double balance, double limit = 1000)
            : BankAccount(person, balance, limit)
        {
            public string TakeCash(double amount)
            {
                if (amount > Limit || amount > Balance)
                    return "Too much";

                return $"Left: {Balance -= amount}";
            }
        }

        public class TaskOnClass1 : ITask
        {
            public void Start()
            {
                l_t1_03_09_2024.Cat cat = new();
                cat.Sound();
                cat.Walk();

                l_t1_03_09_2024.Dog dog = new();
                dog.Sound();
                dog.Walk();

                l_t1_03_09_2024.Snake snake = new();
                snake.Sound();
                snake.Walk();

                StringFeatures.DisplayDashes();

                PrivateBankAccount privateBank = new(new PrivateBankIdentify(), 1_000_000_000, 1_000_000);
                privateBank.Deposit(2);

                Console.WriteLine(privateBank.TakeCash(2_000_00)); ;
                Console.WriteLine(privateBank);
            }
        }
    }
    namespace h_t_04_09_2024
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

        public class Homework1 : ITask
        {
            public void Start()
            {
                Task2.IBankAccount? account;

                while (true)
                {
                    Console.WriteLine("Select account type:");
                    Console.WriteLine("1. Deposit Account");
                    Console.WriteLine("2. Current Account");
                    Console.WriteLine("q. Quit");
                    var choice = Console.ReadLine();

                    if (choice == "q") break;

                    account = choice switch
                    {
                        "1" => CreateDepositAccount(),
                        "2" => CreateCurrentAccount(),
                        _ => null
                    };

                    if (account is null) continue;

                    while (true)
                    {
                        Console.WriteLine("Select operation:\n1. Display Balance\n2. Deposit\n3. Withdraw\nq. Back to Account Selection");
                        var operation = Console.ReadLine();

                        if (operation == "q") break;

                        switch (operation)
                        {
                            case "1":
                                account.DisplayBalance();
                                break;
                            case "2":
                                Console.Write("Enter deposit amount: ");

                                if (double.TryParse(Console.ReadLine(), out double depositAmount))
                                    account.Deposit(depositAmount);
                                else
                                    Console.WriteLine("Invalid amount.");
                                break;
                            case "3":
                                Console.Write("Enter withdraw amount: ");

                                if (double.TryParse(Console.ReadLine(), out double withdrawAmount))
                                    account.Withdraw(withdrawAmount);
                                else
                                    Console.WriteLine("Invalid amount.");
                                break;
                            default:
                                Console.WriteLine("Invalid operation. Try again.");
                                break;
                        }
                    }
                }
            }

            static Task2.IBankAccount CreateDepositAccount()
            {
                Console.Write("Enter account number: ");
                var number = Console.ReadLine();

                Console.Write("Enter account owner: ");
                var owner = Console.ReadLine();

                Console.Write("Enter initial balance: ");
                var balance = double.Parse(Console.ReadLine());

                return new Task2.DepositAccount(number, owner, balance);
            }

            static Task2.IBankAccount CreateCurrentAccount()
            {
                Console.Write("Enter account number: ");
                var number = Console.ReadLine();

                Console.Write("Enter account owner: ");
                var owner = Console.ReadLine();

                Console.Write("Enter initial balance: ");
                var balance = double.Parse(Console.ReadLine());

                return new Task2.CurrentAccount(number, owner, balance);
            }
        }
    }
    namespace l_t_05_09_2024
    {
        namespace Task1
        {
            public class XCoord(double coord)
            {
                public double Coord { get; private set; } = coord;
            }

            public class YCoord(double coord)
            {
                public double Coord { get; private set; } = coord;
            }

            public class Coord(double x, double y)
            {
                public Tuple<XCoord, YCoord> Point { get; set; } = new(new XCoord(x), new YCoord(y));

                public static double operator ^(Coord c1, Coord c2)
                {
                    return Math.Sqrt(Math.Pow(c2.Point.Item1.Coord - c1.Point.Item1.Coord, 2) + Math.Pow(c2.Point.Item2.Coord - c1.Point.Item2.Coord, 2));
                }
            }
        }
        public class TaskOnClass2 : ITask
        {
            public void Start()
            {
                Action task1 = () =>
                {
                    Task1.Coord xyCoord1 = new(12.5, 9);
                    Task1.Coord xyCoord2 = new(-1, 2);

                    Console.WriteLine($"Distance between points: {xyCoord1 ^ xyCoord2}");
                };

                task1.Invoke();
            }
        }
    }
    namespace h_t_06_09_2024
    {
        namespace Task1
        {
            public interface ICoffeeMachine
            {
                uint WaterCapacity { get; }
                uint CoffeeBeansCapacity { get; }
                uint Water { get; }
                uint CoffeeBeans { get; }
                bool IsWaterHeated { get; }
                void GrindBeans(uint coffeeBeans);
                void EspressoRule();
                void UnloadWater(uint amount);
                void LatteRule();
                void LoadWater(uint water);
                void LoadCoffeeBeans(uint coffeeBeans);
                void HeatWater();
            }
            public class CoffeeMachine(uint waterCapacity, uint coffeeBeansCapacity) : ICoffeeMachine
            {
                private uint _water;
                private uint _coffeeBeans;
                public uint Water
                {
                    get => _water;
                    private set
                    {
                        if (value > WaterCapacity)
                            _water = WaterCapacity;
                        else
                            _water = value;

                        if (_water == 0)
                            IsWaterHeated = false;
                    }
                }
                public uint CoffeeBeans
                {
                    get => _coffeeBeans;
                    private set
                    {
                        if (value > CoffeeBeansCapacity)
                            _coffeeBeans = CoffeeBeansCapacity;
                        else
                            _coffeeBeans = value;
                    }
                }
                public bool IsWaterHeated { get; private set; } = false;
                public uint WaterCapacity { get; } = waterCapacity;
                public uint CoffeeBeansCapacity { get; } = coffeeBeansCapacity;

                public void GrindBeans(uint coffeeBeans)
                {
                    if (CoffeeBeans < coffeeBeans)
                        throw new Exception("Wrong amount of coffee beans!");

                    CoffeeBeans -= coffeeBeans;
                }

                public void EspressoRule()
                {
                    GrindBeans(20);


                    bool heating = false;
                    try
                    {
                        if (!IsWaterHeated)
                        {
                            HeatWater();
                            heating = true;
                        }

                        UnloadWater(2000);
                    }
                    catch (Exception)
                    {
                        CoffeeBeans += 20;

                        if (heating)
                            IsWaterHeated = false;

                        throw;
                    }

                    Console.WriteLine("Espresso is ready!");
                }
                public void LatteRule()
                {
                    GrindBeans(25);

                    bool heating = false;
                    try
                    {
                        if (!IsWaterHeated)
                        {
                            HeatWater();
                            heating = true;
                        }
                        UnloadWater(2500);
                    }
                    catch (Exception)
                    {
                        CoffeeBeans += 25;

                        if (heating)
                            IsWaterHeated = false;

                        throw;
                    }

                    Console.WriteLine("Latte is ready!");
                }
                public void LoadWater(uint water)
                {
                    if (Water + water > WaterCapacity)
                        Water = WaterCapacity;
                    else
                        Water += water;
                }

                public void LoadCoffeeBeans(uint coffeeBeans)
                {
                    if (CoffeeBeans + coffeeBeans > CoffeeBeansCapacity)
                        CoffeeBeans = CoffeeBeansCapacity;
                    else
                        CoffeeBeans += coffeeBeans;
                }

                public void HeatWater()
                {
                    if (Water != 0)
                        IsWaterHeated = true;
                    else throw new Exception("Not enough water!");
                }


                public void UnloadWater(uint amount)
                {
                    if (Water < amount)
                        throw new Exception("Not enough water!");

                    Water -= amount;
                }

                public override string ToString()
                    => $"{nameof(Water)}: {Water}\n{nameof(CoffeeBeans).CamelCaseToSpace()}: {CoffeeBeans}\n{nameof(IsWaterHeated).CamelCaseToSpace()}: {IsWaterHeated}\n{nameof(WaterCapacity).CamelCaseToSpace()}: {WaterCapacity}\n{nameof(CoffeeBeansCapacity).CamelCaseToSpace()}: {CoffeeBeansCapacity}";
            }
        }

        namespace Task2
        {
            public interface IDigitalWallet
            {
                double CheckBalance();
                bool Withdraw(double amount);
                void Deposit(double amount);
                List<string> GetTransactionLog();
                void SetAuthProvider(ILoginProvider provider);
            }

            public interface ILoginProvider
            {
                bool Validate(string token, string password);
                string GetToken(string password);
            }

            public class GmailAuthProvider(string gmail, string password) : ILoginProvider
            {
                private readonly string _gmail = gmail;
                private readonly string _password = password.HashPassword();
                public string GetToken(string password)
                    => $"{_gmail}:{password}:{_password}";
                public bool Validate(string token, string password)
                    => token == GetToken(password);
            }

            public class Privat24AuthProvider(string phoneNumber, string password) : ILoginProvider
            {
                private readonly string _phoneNumber = phoneNumber;
                private readonly string _password = password.HashPassword();
                public string GetToken(string password)
                    => $"{_phoneNumber}:{password}:{_password}";
                public bool Validate(string token, string password)
                    => token == GetToken(password);
            }

            public class DigitalWallet : IDigitalWallet
            {
                private double _balance = 0;
                private readonly string _login;
                private readonly string _password;
                private readonly List<string> _transactionLog = [];
                private string token;
                private ILoginProvider _authProvider;

                public DigitalWallet(string login, string password, ILoginProvider authProvider)
                {
                    _login = login;
                    _password = password.HashPassword();
                    _authProvider = authProvider;
                    token = _authProvider.GetToken(_password);
                }

                public double CheckBalance()
                {
                    if (!IsAuthenticated())
                        throw new UnauthorizedAccessException("Invalid credentials");

                    return _balance;
                }

                public bool Withdraw(double amount)
                {
                    if (!IsAuthenticated())
                        throw new UnauthorizedAccessException("Invalid credentials");

                    if (_balance >= amount)
                    {
                        _balance -= amount;
                        _transactionLog.Add($"Withdrawal: {amount}");
                        return true;
                    }

                    return false;
                }

                public void Deposit(double amount)
                {
                    if (!IsAuthenticated())
                        throw new UnauthorizedAccessException("Invalid credentials");

                    _balance += amount;
                    _transactionLog.Add($"Deposit: {amount}");
                }

                public List<string> GetTransactionLog()
                {
                    if (!IsAuthenticated())
                        throw new UnauthorizedAccessException("Invalid credentials");

                    return _transactionLog;
                }

                public void SetAuthProvider(ILoginProvider provider)
                {
                    _authProvider = provider;
                    token = _authProvider.GetToken(_password);
                }

                private bool IsAuthenticated()
                    => _authProvider.Validate(token, _password);
            }
        }

        public class Homework2 : ITask
        {
            public void Start()
            {
                Action action1 = () =>
                {
                    Task1.ICoffeeMachine coffeeMachine = new Task1.CoffeeMachine(10000, 100);

                    while (true)
                    {
                        StringFeatures.DisplayDashes();
                        Console.WriteLine("1. Espresso\n2. Latte\n3. Grind 20 Beans\n4. Load 5000ml Water\n5. Load 20 Coffee Beans\n6. Get Info\n7. Heat Water\nq. Exit");
                        try
                        {
                            switch (Console.ReadLine())
                            {
                                case "1": coffeeMachine.EspressoRule(); break;
                                case "2": coffeeMachine.LatteRule(); break;
                                case "3": coffeeMachine.GrindBeans(20); break;
                                case "4": coffeeMachine.LoadWater(5000); break;
                                case "5": coffeeMachine.LoadCoffeeBeans(20); break;
                                case "6": Console.WriteLine(coffeeMachine); break;
                                case "7": coffeeMachine.HeatWater(); break;
                                case "q": return;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                };

                Action action2 = () =>
                {
                    Task2.IDigitalWallet digitalWallet = new Task2.DigitalWallet("login", "password", new Task2.GmailAuthProvider("login", "password"));

                    while (true)
                    {
                        StringFeatures.DisplayDashes();
                        Console.WriteLine("1. Check Balance\n2. Withdraw 100\n3. Deposit 100\n4. Get Transaction Log\n5. Set Auth Provider\nq. Exit");
                        try
                        {

                            switch (Console.ReadLine())
                            {
                                case "1": Console.WriteLine(digitalWallet.CheckBalance()); break;
                                case "2": Console.WriteLine(digitalWallet.Withdraw(100)); break;
                                case "3": digitalWallet.Deposit(100); break;
                                case "4": Console.WriteLine(string.Join('\n', digitalWallet.GetTransactionLog())); break;
                                case "5": digitalWallet.SetAuthProvider(new Task2.Privat24AuthProvider("login", "password")); break;
                                case "q": return;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                };

                TaskHelper.MakeTasksExecutor(action1, action2);
            }
        }
    }
}