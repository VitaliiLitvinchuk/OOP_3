using System.Text.RegularExpressions;
using FluentAssertions;
using Task.Homework.h_t_30_09_2024.Task;
using Xunit;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework8 : ITask
    {
        public void Start()
        {
            Console.WriteLine("Kata calculator tests");
        }
    }
}

namespace Task.Homework.h_t_30_09_2024
{
    namespace Test
    {
        public class UnitTest
        {
            private readonly ILogger _logger = new FileLogger();
            [Theory]
            [InlineData("%$1#&!3&*4(-!#2);/.'.'", 10)]
            public void ShouldCalculate(string input, int expected)
            {
                var calc = new StringCalculator(_logger);

                var result = calc.Add(input);

                result.Should().Be(expected);
            }

            [Theory]
            [InlineData("", 0)]
            public void ShouldReturnZero(string input, int expected)
            {
                var calc = new StringCalculator(_logger);

                var result = calc.Add(input);

                result.Should().Be(expected);
            }

            [Theory]
            [InlineData("1001 1", 1)]
            public void ShouldSkipBigNumbers(string input, int expected)
            {
                var calc = new StringCalculator(_logger);

                var result = calc.Add(input);

                result.Should().Be(expected);
            }

            [Theory]
            [InlineData("-12413, -1213, 1")]
            public void ShouldThrowException(string input)
            {
                var calc = new StringCalculator(_logger);

                Assert.Throws<Exception>(() => calc.Add(input)).Message.Should().Be("negatives not allowed: -12413, -1213");
            }
        }
    }

    namespace Task
    {
        public class StringCalculator(ILogger logger)
        {
            public string Rgx = @"[^-\d]+";
            public int Add(string input)
            {
                logger.LogInfo($"Add {input}");
                string[] stringNumbers = Regex.Replace(input, Rgx, " ").Split(" ", StringSplitOptions.RemoveEmptyEntries);

                logger.LogInfo($"Formatting Numbers: {string.Join(", ", stringNumbers)}");
                int[] numbers = [.. stringNumbers.Where(x => int.TryParse(x, out _)).Select(int.Parse)];

                logger.LogInfo($"Search Bellow Zero Numbers: {string.Join(", ", numbers)}");
                int[] belowZero = [.. numbers.Where(x => x < 0)];

                if (belowZero.Length != 0)
                {
                    logger.LogInfo($"Throw Exception: {string.Join(", ", belowZero)}");
                    throw new Exception($"negatives not allowed: {string.Join(", ", belowZero)}");
                }

                logger.LogInfo($"Sum Numbers: {string.Join(", ", numbers)}");
                return numbers.Where(x => x <= 1000).Sum();
            }
        }

        public interface ILogger
        {
            void LogInfo(string message);
        }

        public class FileLogger : ILogger
        {
            public void LogInfo(string message)
            {
                File.AppendAllLines($"{Directory.GetCurrentDirectory()}/log.txt", [message]);
            }
        }
    }
}