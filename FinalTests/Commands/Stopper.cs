using Task.FinalTests.Commands.Abstracts;

namespace Task.FinalTests.Commands
{
    public class Stopper(IIOWrapper wrapper) : ICommand
    {
        public bool Continue => false;

        public void Execute()
        {
            wrapper.OutValue("Stopped!");
        }
    }
}
