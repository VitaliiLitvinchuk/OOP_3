using Task.FinalTests.Commands.Abstracts;
using Task.FinalTests.Zippers.Abstracts;
using Task.FinalTests.Zippers;
using Task.FinalTests.Executor.Abstracts;

namespace Task.FinalTests.Commands
{
    public class UnzipArchive(IIOWrapper wrapper, IZipperExecutor executor) : ICommand
    {
        public bool Continue => true;
        public async void Execute()
        {
            wrapper.OutValue("Enter full path to source archive");
            string pathFrom = wrapper.InValue();

            wrapper.OutValue("Enter full path to destination folder");
            string pathTo = wrapper.InValue();

            IPathExecutor zipper = new Unzipper(pathFrom, pathTo);

            executor.Zipper = zipper;
            await executor.DoZipAction();
        }
    }
}
