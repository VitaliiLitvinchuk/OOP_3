using Task.FinalTests.Executor.Abstracts;
using Task.FinalTests.Zippers.Abstracts;

namespace Task.FinalTests.Executor
{
    public class ZipperExecutor : IZipperExecutor
    {
        public IPathExecutor? Zipper { get; set; } = null;
    }
}