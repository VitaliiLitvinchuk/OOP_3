using System.IO.Compression;
using Task.FinalTests.Zippers.Abstracts;

namespace Task.FinalTests.Zippers
{
    public class Unzipper(string pathFrom, string pathTo) : IPathExecutor
    {
        public string PathFrom { get; private set; } = pathFrom;
        public string PathTo { get; private set; } = pathTo;

        public async System.Threading.Tasks.Task ExecuteAsync()
        {
            if (!File.Exists(PathFrom))
                throw new FileNotFoundException($"Zip file not found: {PathFrom}");

            if (!Directory.Exists(PathTo))
                Directory.CreateDirectory(PathTo);

            await System.Threading.Tasks.Task.Run(() =>
                ZipFile.ExtractToDirectory(PathFrom, PathTo)
            );
        }
    }
}