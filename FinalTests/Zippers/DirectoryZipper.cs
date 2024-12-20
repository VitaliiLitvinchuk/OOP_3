using System.IO.Compression;
using Task.FinalTests.Zippers.Abstracts;

namespace Task.FinalTests.Zippers
{
    public class DirectoryZipper(string pathFrom, string pathTo, CompressionLevel compressionLevel) : IPathExecutor
    {
        public string PathFrom { get; private set; } = pathFrom;
        public string PathTo { get; private set; } = pathTo;
        public CompressionLevel CompressionLevel { get; private set; } = compressionLevel;

        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            if (!Directory.Exists(PathFrom))
                throw new DirectoryNotFoundException($"Directory not found: {PathFrom}");

            await System.Threading.Tasks.Task.Run(() =>
                ZipFile.CreateFromDirectory(PathFrom, PathTo, CompressionLevel, includeBaseDirectory: true)
            );
        }
    }
}