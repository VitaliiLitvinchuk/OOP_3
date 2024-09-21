namespace Task
{
    public class Program
    {
        static void Main(string[] args)
        {
            TasksWorkers.Worker.Start(TasksWorkers.Worker.GetCount());

            // TasksWorkers.Worker.Namespace = "Task.Class";
            // TasksWorkers.Worker.Start(TasksWorkers.Worker.GetCount());
        }
    }
}
