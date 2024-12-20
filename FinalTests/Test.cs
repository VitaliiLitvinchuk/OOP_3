using Microsoft.Extensions.DependencyInjection;
using Task.FinalTests.Commands;
using static Task.TasksWorkers;

///
/// IoC
/// Commands: ZipFileCommand (true) - A, ZipFolderCommand (true) - B, UnzipArchive (true) - U, Stopper (false) - E ->
/// Executor: ZipperExecutor (zip file, zip folder, unzip archive) ->
///     Zippers (pattern strategy): FileZipper, FolderZipper, Unzipper
///

namespace Task.FinalTests
{
    enum Keys { A, B, U, E }
    public partial class Test : ITask
    {
        public void Start()
        {
            Dictionary<Keys, Func<bool>> actions = new()
            {
                { Keys.A,
                () => {
                    var zipFile = IoC.ServiceProvider.GetRequiredService<ZipFileCommand>();
                    zipFile.Execute();
                    return zipFile.Continue;
                } },
                { Keys.B,
                () => {
                    var zipFolder = IoC.ServiceProvider.GetRequiredService<ZipFolderCommand>();
                    zipFolder.Execute();
                    return zipFolder.Continue;
                } },
                { Keys.U,
                () => {
                    var unzip = IoC.ServiceProvider.GetRequiredService<UnzipArchive>();
                    unzip.Execute();
                    return unzip.Continue;
                } },
                { Keys.E,
                () => {
                    var stopper = IoC.ServiceProvider.GetRequiredService<Stopper>();
                    stopper.Execute();
                    return stopper.Continue;
                } }
            };

            bool cont = true;
            while (cont)
            {
                Console.WriteLine($"Enter key: {string.Join(", ", actions.Keys)}");
                Keys key = (Keys)Enum.Parse(typeof(Keys), Console.ReadLine()!, true);
                Console.WriteLine(key);
                cont = actions[key]();
            }
        }
    }
}
