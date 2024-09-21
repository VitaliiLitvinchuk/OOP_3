using Task.Features;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework3 : ITask
    {
        public void Start()
        {
            h_t_09_09_2024.Task.IBankAccount account = new h_t_09_09_2024.Task.BankAccount();
            h_t_09_09_2024.Task.PaymentProcessor.ProcessPayment(account);

            account = new h_t_09_09_2024.Task.PayoneerAccount();
            h_t_09_09_2024.Task.PaymentProcessor.ProcessPayment(account);

            account = new h_t_09_09_2024.Task.WiseAccount();
            h_t_09_09_2024.Task.PaymentProcessor.ProcessPayment(account);
        }
    }
}

namespace Task.Homework.h_t_09_09_2024
{
    namespace Task
    {
        public interface IBankAccount
        {
            void DoSomeLogic();
        }

        public class BankAccount : IBankAccount
        {
            public void DoSomeLogic()
            {
                Console.WriteLine($"{typeof(BankAccount).Name.CamelCaseToSpace()} is doing some logic...");
            }
        }

        public class PayoneerAccount : IBankAccount
        {
            public void DoSomeLogic()
            {
                Console.WriteLine($"{typeof(PayoneerAccount).Name.CamelCaseToSpace()} is doing some logic...");
            }
        }

        public class WiseAccount : IBankAccount
        {
            public void DoSomeLogic()
            {
                Console.WriteLine($"{typeof(WiseAccount).Name.CamelCaseToSpace()} is doing some logic...");
            }
        }

        public class PaymentProcessor
        {
            public static void ProcessPayment(IBankAccount account)
            {
                StringFeatures.DisplayDashes();
                Console.WriteLine($"Connecting to bank...");
                account.DoSomeLogic();
                Console.WriteLine($"Receiving some cash...");
                Console.WriteLine($"Disconnecting from bank...");
            }
        }
    }
}