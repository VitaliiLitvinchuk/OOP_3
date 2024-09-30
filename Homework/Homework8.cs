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
            [Theory]
            [InlineData("asdasd1,asqwe]2\nasdas 3.dasdaas 4 ssda5!#!@ZCASlkazm/z.qw", 15)]
            public void ShouldSumNumbers(string input, double expected)
            {
                // Arrange
                var calculator = new Calculator();

                // Act
                var result = calculator.Add(input);

                // Assert
                result.Should().Be(expected);
            }

            [Theory]
            [InlineData("", 0)]
            public void ShouldReturnZero(string input, double expected)
            {
                // Arrange
                var calculator = new Calculator();

                // Act
                var result = calculator.Add(input);

                // Assert
                result.Should().Be(expected);
            }

            [Theory]
            [InlineData("10000, 231241, 1000, 1001 2", 2)]
            public void ShouldSkipBigNumbers(string input, double expected)
            {
                // Arrange
                var calculator = new Calculator();

                // Act
                var result = calculator.Add(input);

                // Assert
                result.Should().Be(expected);
            }

            [Theory]
            [InlineData("-10000, 231241, -1000, 1001 2")]
            public void ShouldThrowException(string input)
            {
                // Arrange
                var calculator = new Calculator();

                // Act and Assert
                Assert.Throws<Exception>(() => calculator.Add(input));
            }
        }
    }

    namespace Task
    {
        public class Calculator
        {
            public string AbsoluteRegex { get; set; } = @"[^-\d]+";
            public double Add(string values)
            {
                var numbers = Regex.Replace(values, AbsoluteRegex, " ")
                                   .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                   .Select(double.Parse);
                double sum = 0;

                if (numbers.Any(number => number < 0))
                    throw new Exception($"Negative numbers are not allowed: {string.Join(", ", numbers
                                                .Where(number => number < 0))}");

                foreach (var number in numbers)
                    if (number < 1000)
                        sum += number;

                return sum;
            }
        }
    }
}