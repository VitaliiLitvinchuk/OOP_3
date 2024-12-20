namespace Task.FinalTests.Commands.Abstracts
{
    public interface ICommand : IContinuer
    {
        void Execute();
    }
}
