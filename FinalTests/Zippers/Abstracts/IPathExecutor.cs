namespace Task.FinalTests.Zippers.Abstracts
{
    public interface IPathExecutor
    {
        string PathTo { get; }
        string PathFrom { get; }
        System.Threading.Tasks.Task ExecuteAsync();
    }
}