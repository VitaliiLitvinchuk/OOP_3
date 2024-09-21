using Task.Class.l_t_03_09_2024;
using Task.Features;
using static Task.TasksWorkers;

namespace Task.Class
{
    public class TaskOnClass1 : ITask
    {
        public void Start()
        {
            Cat cat = new();
            cat.Sound();
            cat.Walk();

            Dog dog = new();
            dog.Sound();
            dog.Walk();

            Snake snake = new();
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

namespace Task.Class.l_t_03_09_2024
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
}