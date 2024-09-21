using Task.Features;
using Task.Homework.h_t_18_09_2024.Account;
using Task.Homework.h_t_18_09_2024.Car;
using Task.Homework.h_t_18_09_2024.CarDealer;
using Task.Homework.h_t_18_09_2024.Inventory;
using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework4 : ITask
    {
        public IEnumerable<Car> GetRandomCars()
        {
            return Enumerable.Range(0, 20).Select(x => new JustCar() { Price = TaskHelper.random.Next(10000) }).Cast<Car>();
        }
        private List<Car> MyCars = [];
        private static readonly string password = "1";
        public void Start()
        {
            Console.Write("Enter password: ");
            var passwordInput = Console.ReadLine();
            bool isAdmin = passwordInput == password;

            ICarDealer carDealer1 = new CarDealer(new Inventory(), new CurrentAccount(100_000), 5);

            foreach (var car in GetRandomCars())
            {
                if (car.Price > carDealer1.Account.Balance)
                    continue;

                carDealer1.BuyCar(car);
            }

            ICarDealer carDealer2 = new CarDealer(new Inventory(), new CurrentAccount(1_000_000), 7.5);

            foreach (var car in GetRandomCars())
            {
                if (car.Price > carDealer2.Account.Balance)
                    continue;

                carDealer2.BuyCar(car);
            }

            var actions = new Dictionary<string, Action>();

            if (isAdmin)
            {
                actions.Add("Car dealer 1", () =>
                {
                    Console.WriteLine("Car dealer 1:");
                    Console.WriteLine("Balance: " + carDealer1.Account.Balance);
                    foreach (var car in carDealer1.GetCars())
                        Console.WriteLine(car);
                });
                actions.Add("Car dealer 2", () =>
                {
                    Console.WriteLine("Car dealer 2:");
                    Console.WriteLine("Balance: " + carDealer2.Account.Balance);
                    foreach (var car in carDealer2.GetCars())
                        Console.WriteLine(car);
                });
                actions.Add("Exchange cars", () =>
                {
                    if (long.TryParse(Console.ReadLine(), out long id1))
                        if (long.TryParse(Console.ReadLine(), out long id2))
                        {
                            var car1 = carDealer1.GetCarById(id1);

                            if (car1 == null)
                            {
                                Console.WriteLine("Car not found");
                                return;
                            }

                            var car2 = carDealer2.GetCarById(id2);

                            if (car2 == null)
                            {
                                Console.WriteLine("Car not found");
                                return;
                            }

                            if (carDealer1.ExchangeCar(car1, car2, carDealer2))
                                Console.WriteLine("Car exchanged");
                            else
                                Console.WriteLine("Car not exchanged");
                        }
                });
            }
            actions.Add("Get cars in budget ", () =>
            {
                if (double.TryParse(Console.ReadLine(), out double budget))
                {
                    Console.WriteLine("Car dealer 1:");
                    foreach (var car in carDealer1.GetCars(budget))
                        Console.WriteLine(car);
                    Console.WriteLine("Car dealer 2:");
                    foreach (var car in carDealer2.GetCars(budget))
                        Console.WriteLine(car);
                }
            });
            actions.Add("Buy car from dealer 1", () =>
            {
                if (long.TryParse(Console.ReadLine(), out long id))
                {
                    var car = carDealer1.GetCarById(id);

                    if (car == null)
                    {
                        Console.WriteLine("Car not found");
                        return;
                    }

                    carDealer1.SellCar(car, car.Price);
                    MyCars.Add(car);
                    Console.WriteLine("Car bought");
                }
            });
            actions.Add("Buy car dealer 2", () =>
            {
                if (long.TryParse(Console.ReadLine(), out long id))
                {
                    var car = carDealer2.GetCarById(id);

                    if (car == null)
                    {
                        Console.WriteLine("Car not found");
                        return;
                    }

                    carDealer2.SellCar(car, car.Price);
                    MyCars.Add(car);
                    Console.WriteLine("Car bought");
                }
            });
            actions.Add("Show my cars", () =>
            {
                Console.WriteLine("My cars:");
                foreach (var car in MyCars)
                    Console.WriteLine(car);
            });

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
        }
    }
}

namespace Task.Homework.h_t_18_09_2024
{
    namespace CarDealer
    {
        public interface ICarDealer
        {
            Inventory.Inventory Inventory { get; }
            IAccount Account { get; }
            double Markup { get; }
            bool BuyCar(Car.Car car);
            bool SellCar(Car.Car car, double cash);
            Car.Car? GetCarById(long id);
            bool ExchangeCar(Car.Car car, Car.Car otherCar, ICarDealer otherDealer);
            List<Car.Car> GetCars(double budget = double.MaxValue);
        }

        public class CarDealer(Inventory.Inventory inventory, IAccount account, double markup) : ICarDealer
        {
            public Inventory.Inventory Inventory { get; } = inventory;
            public IAccount Account { get; } = account;
            public double Markup { get; } = markup;
            public List<Car.Car> GetCars(double budget = double.MaxValue)
                => Inventory.Cars
                .Where(car => car.Price <= budget)
                .ToList();
            public Car.Car? GetCarById(long id)
                => Inventory.Cars.FirstOrDefault(car => car.Id == id);
            public bool BuyCar(Car.Car car)
            {
                if (Account.Balance < car.Price)
                    return false;

                Account.Balance -= car.Price;
                car.Price *= (1 + Markup / 100);
                Inventory.AddCar(car);

                return true;
            }
            public bool SellCar(Car.Car car, double cash)
            {
                if (!Inventory.Exist(car))
                    return false;

                if (cash < car.Price)
                    return false;

                Account.Balance += cash;
                Inventory.RemoveCar(car);
                return true;
            }
            public bool ExchangeCar(Car.Car car, Car.Car otherCar, ICarDealer otherDealer)
            {
                if (!Inventory.Exist(car) || !otherDealer.Inventory.Exist(otherCar))
                    return false;

                double diff = car.Price - otherCar.Price;

                if (Account.Balance < diff || otherDealer.Account.Balance < Math.Abs(diff))
                    return false;

                Account.Balance += diff;
                otherDealer.Account.Balance -= diff;

                Inventory.RemoveCar(car);
                otherDealer.Inventory.RemoveCar(otherCar);

                Inventory.AddCar(otherCar);
                otherDealer.Inventory.AddCar(car);

                return true;
            }
        }
    }

    namespace Car
    {
        public class Car
        {
            public virtual long Id { get; }
            public double Price { get; set; }
        }

        public class JustCar : Car
        {
            private readonly JustCarId JustCarId = JustCarId.New();
            public override long Id { get => JustCarId.Value; }
            public override string ToString()
                => $"Id: {Id} Price: {Price:#.##}";
        }

        public class JustCarId
        {
            private JustCarId()
            {
                Value = ++total;
            }
            private static long total = 0;
            public long Value { get; private set; }
            public static JustCarId New()
                => new();
        }
    }

    namespace Inventory
    {
        public class Inventory
        {
            private readonly List<Car.Car> _cars = [];
            public List<Car.Car> Cars => _cars;
            public void AddCar(Car.Car car)
                => _cars.Add(car);
            public void RemoveCar(Car.Car car)
                => _cars.Remove(car);
            public bool Exist(Car.Car car)
                => _cars.Contains(car);
        }

    }
    namespace Account
    {
        public interface IAccount
        {
            double Balance { get; set; }
        }

        public class CurrentAccount(double balance) : IAccount
        {
            public double Balance { get; set; } = balance;
        }
    }
}