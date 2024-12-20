using System.IO.Compression;
using Task.FinalTests.Commands.Abstracts;
using Task.FinalTests.Executor.Abstracts;
using Task.FinalTests.Zippers;
using Task.FinalTests.Zippers.Abstracts;

namespace Task.FinalTests.Commands
{
    public class ZipFileCommand(IIOWrapper wrapper, IZipperExecutor executor) : ICommand
    {
        public bool Continue => true;
        public async void Execute()
        {
            wrapper.OutValue("Enter full path to source file");
            string pathFrom = wrapper.InValue();

            wrapper.OutValue("Enter full path to destination folder");
            string pathTo = wrapper.InValue();

            wrapper.OutValue($"Enter comprassion level {string.Join(", ", Enum.GetNames(typeof(CompressionLevel)))}");
            CompressionLevel compressionLevel = (CompressionLevel)Enum.Parse(typeof(CompressionLevel), wrapper.InValue(), true);

            IPathExecutor zipper = new FileZipper(pathFrom, pathTo, compressionLevel);

            executor.Zipper = zipper;
            await executor.DoZipAction();
        }
    }
}
