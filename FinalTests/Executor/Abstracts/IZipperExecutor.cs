using Task.FinalTests.Zippers.Abstracts;

namespace Task.FinalTests.Executor.Abstracts
{
    public interface IZipperExecutor
    {
        public IPathExecutor? Zipper { get; set; }

        public async System.Threading.Tasks.Task DoZipAction()
        {
            if (Zipper is null)
                throw new ArgumentNullException(nameof(Zipper));

            await Zipper.ExecuteAsync();
        }
    }
}
