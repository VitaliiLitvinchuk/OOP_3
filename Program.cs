using static Task.TasksWorkers;

namespace Task
{
    public class Program
    {
        static void Main(string[] args)
        {
            Homework.Start(3);
            // TaskOnClass.Start(3);
        }
    }
}
