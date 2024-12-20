using Task.Features;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework2 : ITask
    {
        public void Start()
        {
            Action action1 = () =>
            {
                h_t_06_09_2024.Task1.ICoffeeMachine coffeeMachine = new h_t_06_09_2024.Task1.CoffeeMachine(10000, 100);

                var actions = new Dictionary<string, Action>
                {
                        { "Espresso", () => coffeeMachine.EspressoRule() },
                        { "Latte", () => coffeeMachine.LatteRule() },
                        { "Grind 20 Beans", () => coffeeMachine.GrindBeans(20) },
                        { "Load 5000ml Water", () => coffeeMachine.LoadWater(5000) },
                        { "Load 20 Coffee Beans", () => coffeeMachine.LoadCoffeeBeans(20) },
                        { "Get Info", () => Console.WriteLine(coffeeMachine) },
                        { "Heat Water", () => coffeeMachine.HeatWater() }
                };

                while (true)
                {
                    StringFeatures.DisplayDashes();
                    try
                    {
                        if (!TaskHelper.MakeTasksExecutor(actions))
                            break;

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            };

            Action action2 = () =>
            {
                h_t_06_09_2024.Task2.IDigitalWallet digitalWallet = new h_t_06_09_2024.Task2.DigitalWallet("login", "password", new h_t_06_09_2024.Task2.GmailAuthProvider("login", "password"));

                var actions = new Dictionary<string, Action>
                {
                        { "Check Balance", () => Console.WriteLine(digitalWallet.CheckBalance()) },
                        { "Withdraw 100", () => Console.WriteLine(digitalWallet.Withdraw(100)) },
                        { "Deposit 100", () => digitalWallet.Deposit(100) },
                        { "Get Transaction Log", () => Console.WriteLine(string.Join('\n', digitalWallet.GetTransactionLog())) },
                        { "Set Auth Provider", () => digitalWallet.SetAuthProvider(new h_t_06_09_2024.Task2.Privat24AuthProvider("login", "password")) },
                };

                while (true)
                {
                    StringFeatures.DisplayDashes();
                    try
                    {
                        if (!TaskHelper.MakeTasksExecutor(actions))
                            break;
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

namespace Task.Homework.h_t_06_09_2024
{
    namespace Task1
    {
        public interface ICoffeeMachine
        {
            public static readonly uint LatteBeans = 25;
            public static readonly uint EspressoBeans = 20;
            public static readonly uint WaterLatte = 2500;
            public static readonly uint WaterEspresso = 2000;
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

                    if (_water == uint.MinValue)
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
                GrindBeans(ICoffeeMachine.EspressoBeans);

                bool heating = false;
                try
                {
                    if (!IsWaterHeated)
                    {
                        HeatWater();
                        heating = true;
                    }

                    UnloadWater(ICoffeeMachine.WaterEspresso);
                }
                catch (Exception)
                {
                    CoffeeBeans += ICoffeeMachine.EspressoBeans;

                    if (heating)
                        IsWaterHeated = false;

                    throw;
                }

                Console.WriteLine("Espresso is ready!");
            }
            public void LatteRule()
            {
                GrindBeans(ICoffeeMachine.LatteBeans);

                bool heating = false;
                try
                {
                    if (!IsWaterHeated)
                    {
                        HeatWater();
                        heating = true;
                    }
                    UnloadWater(ICoffeeMachine.WaterLatte);
                }
                catch (Exception)
                {
                    CoffeeBeans += ICoffeeMachine.LatteBeans;

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
                if (Water != uint.MinValue)
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
            private string _token;
            private ILoginProvider _authProvider;

            public DigitalWallet(string login, string password, ILoginProvider authProvider)
            {
                _login = login;
                _password = password.HashPassword();
                _authProvider = authProvider;
                _token = _authProvider.GetToken(_password);
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
                _token = _authProvider.GetToken(_password);
            }

            private bool IsAuthenticated()
                => _authProvider.Validate(_token, _password);
        }
    }
}