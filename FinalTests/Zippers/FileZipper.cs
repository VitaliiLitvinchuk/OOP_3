using System.IO.Compression;
using Task.FinalTests.Zippers.Abstracts;

namespace Task.FinalTests.Zippers
{
    public class FileZipper(string pathFrom, string pathTo, CompressionLevel compressionLevel) : IPathExecutor
    {
        public string PathFrom { get; private set; } = pathFrom;
        public string PathTo { get; private set; } = pathTo;
        public CompressionLevel CompressionLevel { get; private set; } = compressionLevel;

        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            if (!File.Exists(PathFrom))
                throw new FileNotFoundException($"File not found: {PathFrom}");

            string zipFilePath = Path.Combine(PathTo, $"{Path.GetFileNameWithoutExtension(PathFrom)}.zip");

            await System.Threading.Tasks.Task.Run(() =>
            {
                using FileStream zipToOpen = new(zipFilePath, FileMode.Create);
                using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Create);

                archive.CreateEntryFromFile(PathFrom, Path.GetFileName(PathFrom), CompressionLevel);
            });
        }
    }
}