using static Task.TasksWorkers;

namespace Task.Homework
{
    public class Homework6 : ITask
    {
        public void Start()
        {
            var numbers = "a, 1, 2, f, -1, 0, 4, 10, 4, f, 4f, 8, 9, 3"
                                            .Split(", ".Split(), StringSplitOptions.TrimEntries)
                                            .Where(s => double.TryParse(s, out double d))
                                            .OrderBy(double.Parse)
                                            .Skip(3);

            Console.WriteLine(string.Join(", ", numbers));

            Console.WriteLine(numbers.Select(double.Parse)
                                     .Sum());
        }
    }
}

namespace Task.Homework.h_t_26_09_2024
{
}
