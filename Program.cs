using System.Text;

namespace Task
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            TasksWorkers.Worker.Start(TasksWorkers.Worker.GetCount() - 5);

            // TasksWorkers.Worker.Namespace = "Task.Class";
            // TasksWorkers.Worker.Start(3);

            // TasksWorkers.Worker.Namespace = "Task.FinalTests";
            // TasksWorkers.Worker.Start(TasksWorkers.Worker.GetCount());
        }
    }
}
