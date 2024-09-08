namespace Task;

public class TasksWorkers
{
    public interface ITask
    {
        void Start();
    }

    public static class Homework
    {
        private static readonly IEnumerable<ITask> _homeworks = [
            new h_t_04_09_2024.Homework1(),
            new h_t_06_09_2024.Homework2(),
        ];
        public static void Start(int number)
        {
            _homeworks.ElementAt(number - 1).Start();
        }
    }

    public static class TaskOnClass
    {
        private static readonly IEnumerable<ITask> _onClass = [
            new l_t2_03_09_2024.TaskOnClass1(),
            new l_t_05_09_2024.TaskOnClass2(),
        ];
        public static void Start(int number)
        {
            _onClass.ElementAt(number - 1).Start();
        }
    }
}
