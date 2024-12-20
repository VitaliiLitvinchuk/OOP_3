using Microsoft.Extensions.DependencyInjection;
using Task.FinalTests.Executor.Abstracts;
using Task.FinalTests.Zippers;
using Task.FinalTests.Zippers.Abstracts;
using Xunit;

namespace Task.FinalTests
{
    public class UnitTests
    {
        [Theory]
        [InlineData(@"E:\trash_tests")]
        public async System.Threading.Tasks.Task TestAsync(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory not found: {path}");

            if (!File.Exists(Path.Combine(path, "test.txt")))
                throw new FileNotFoundException($"File not found: {Path.Combine(path, "test.txt")}");


            var executor = IoC.ServiceProvider.GetRequiredService<IZipperExecutor>();
            IPathExecutor zipper = new FileZipper(Path.Combine(path, "test.txt"), path, System.IO.Compression.CompressionLevel.SmallestSize);

            {
                executor.Zipper = zipper;
                await executor.DoZipAction();
            }

            zipper = new Unzipper(Path.Combine(path, "test.zip"), Path.Combine(path, "test_folder"));

            {
                executor.Zipper = zipper;
                await executor.DoZipAction();
            }

            zipper = new DirectoryZipper(Path.Combine(path, "test_folder"), Path.Combine(path, "test_folder.zip"), System.IO.Compression.CompressionLevel.SmallestSize);

            {
                executor.Zipper = zipper;
                await executor.DoZipAction();
            }
        }
    }
}