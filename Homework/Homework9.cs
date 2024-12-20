using System.Numerics;
using Task.Homework.h_t_09_10_2024.Task;
using Xunit;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework9 : ITask
    {
        public void Start()
        {
            Calculator<int> intCalc = new();
            IntCalcExecutor(intCalc);

            Calculator<double> doubleCalc = new();
            DoubleCalcExecutor(doubleCalc);

            Calculator<decimal> decimalCalc = new();
            DecimalCalcExecutor(decimalCalc);
        }

        public void IntCalcExecutor(Calculator<int> calculator)
        {
            Console.WriteLine(calculator.Add(10, 20));
            Console.WriteLine(calculator.Subtract(10, 20));
            Console.WriteLine(calculator.Multiply(10, 20));
            Console.WriteLine(calculator.Divide(10, 20));
            Console.WriteLine(calculator.Pow(10, 2));
        }

        public void DoubleCalcExecutor(Calculator<double> calculator)
        {
            Console.WriteLine(calculator.Add(10.1, 20.2));
            Console.WriteLine(calculator.Subtract(10.1, 20.2));
            Console.WriteLine(calculator.Multiply(10.1, 20.2));
            Console.WriteLine(calculator.Divide(10.1, 20.2));
            Console.WriteLine(calculator.Pow(10.1, 3));
        }

        public void DecimalCalcExecutor(Calculator<decimal> calculator)
        {
            Console.WriteLine(calculator.Add(10.1m, 20.2m));
            Console.WriteLine(calculator.Subtract(10.1m, 20.2m));
            Console.WriteLine(calculator.Multiply(10.1m, 20.2m));
            Console.WriteLine(calculator.Divide(10.1m, 20.2m));
            Console.WriteLine(calculator.Pow(10.1m, 3));
        }
    }
}

namespace Task.Homework.h_t_09_10_2024
{
    namespace Test
    {
        public class UnitTest
        {
            [Theory]
            [InlineData(10, 20, 30)]
            [InlineData(10.1, 20.2, 30.3)]
            public void ShouldAddNumbers(double number1, double number2, double expected)
            {
                // Arrange
                Calculator<double> calc = new();

                // Act
                double result = calc.Add(number1, number2);

                // Assert
                Assert.Equal(Math.Round(expected), Math.Round(result));
            }

            [Theory]
            [InlineData(10, 20, -10)]
            [InlineData(10.1, 20.2, -10.1)]
            public void ShouldSubtractNumbers(double number1, double number2, double expected)
            {
                // Arrange
                Calculator<double> calc = new();

                // Act
                double result = calc.Subtract(number1, number2);

                // Assert
                Assert.Equal(Math.Round(expected), Math.Round(result));
            }

            [Theory]
            [InlineData(10, 20, 200)]
            [InlineData(10.1, 20.2, 204.02)]
            public void ShouldMultiplyNumbers(double number1, double number2, double expected)
            {
                // Arrange
                Calculator<double> calc = new();

                // Act
                double result = calc.Multiply(number1, number2);

                // Assert
                Assert.Equal(Math.Round(expected), Math.Round(result));
            }

            [Theory]
            [InlineData(10, 20, 0.5)]
            [InlineData(10.1, 20.2, 0.5)]
            public void ShouldDivideNumbers(double number1, double number2, double expected)
            {
                // Arrange
                Calculator<double> calc = new();

                // Act
                double result = calc.Divide(number1, number2);

                // Assert
                Assert.Equal(Math.Round(expected), Math.Round(result));
            }

            [Fact]
            public void ShouldBeInfinity()
            {
                // Arrange
                Calculator<double> calc = new();

                // Act
                double result = calc.Divide(1, 0);

                // Assert
                Assert.True(double.IsInfinity(result));
            }

            [Theory]
            [InlineData(10, 2, 100)]
            [InlineData(10.1, 2, 102.01)]
            [InlineData(10, -1, 1)]
            public void ShouldPowNumbers(double number1, int number2, double expected)
            {
                // Arrange
                Calculator<double> calc = new();

                // Act
                double result = calc.Pow(number1, number2);

                // Assert
                Assert.Equal(Math.Round(expected), Math.Round(result));
            }

            [Fact]
            public void ShouldThrowInvalidOperationException()
            {
                Assert.Throws<InvalidOperationException>(() => new Calculator<long>());
            }
        }
    }

    namespace Task
    {
        public class Calculator<T> where T : INumber<T>
        {
            private static readonly Type[] SupportedTypes = [typeof(int), typeof(float), typeof(double), typeof(decimal)];

            private void ValidType()
            {
                if (!SupportedTypes.Contains(typeof(T)))
                    throw new InvalidOperationException("Unsupported type");
            }

            public Calculator()
            {
                ValidType();
            }

            public T Add(T number1, T number2)
            {
                return number1 + number2;
            }

            public T Subtract(T number1, T number2)
            {
                return number1 - number2;
            }

            public T Multiply(T number1, T number2)
            {
                return number1 * number2;
            }

            public T Divide(T number1, T number2)
            {
                return number1 / number2;
            }

            public T Pow(T number, int power)
            {
                if (power < 1)
                    return T.One;

                for (int i = 1; i < power; i++)
                    number *= number;

                return number;
            }
        }
    }
}