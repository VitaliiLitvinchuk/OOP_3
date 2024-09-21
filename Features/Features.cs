using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Task.Features
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
        public static readonly Random random = new();
        public static void MakeTasksExecutor(params Action[] actions)
        {
            Console.WriteLine($"Enter number of task(1-{actions.Length})");

            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= actions.Length)
                actions[choice - 1].Invoke();
            else
                Console.WriteLine("Incorrect number");
        }

        public static bool MakeTasksExecutor(Dictionary<string, Action> actions)
        {
            Console.WriteLine($"Type a command:\n{string.Join('\n', actions.Select(action => $" - {action.Key}"))}");

            string? command = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(command) || "quit".StartsWith(command, StringComparison.CurrentCultureIgnoreCase))
                return false;

            foreach (var action in actions)
            {
                if (action.Key.StartsWith(command, StringComparison.OrdinalIgnoreCase))
                {
                    action.Value.Invoke();
                    return true;
                }
                foreach (var commandPart in action.Key.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    if (commandPart.StartsWith(command, StringComparison.OrdinalIgnoreCase))
                    {
                        action.Value.Invoke();
                        return true;
                    }
            }

            Console.WriteLine("Command not found");

            return true;
        }
    }
}