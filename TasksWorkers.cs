namespace Task;

public class TasksWorkers
{
    public interface ITask
    {
        void Start()
        {
            Console.WriteLine("Nothing!");
        }
    }

    public static class Worker
    {
        public static string Namespace { get; set; } = "Task.Homework";
        private static readonly IEnumerable<ITask> _task = [];
        private static readonly int _count = 0;
        static Worker()
        {
            _task = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .Where(t => t.IsClass && !t.IsAbstract && typeof(ITask).IsAssignableFrom(t) && t.Namespace == Namespace)
                        .OrderBy(t => t.Name)
                        .Select(Activator.CreateInstance)
                        .Cast<ITask>();
            _count = _task.Count();
        }
        public static void Start(int number)
        {
            _task.ElementAt(number - 1).Start();
        }
        public static int GetCount()
        {
            return _count;
        }
    }
}
