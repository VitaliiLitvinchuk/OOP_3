using Microsoft.Extensions.DependencyInjection;
using Task.FinalTests.Commands;
using Task.FinalTests.Executor;
using Task.FinalTests.Executor.Abstracts;

namespace Task.FinalTests;

public static class IoC
{
    public static IServiceProvider ServiceProvider { get; } = new ServiceCollection()
                .AddSingleton<IIOWrapper, IOWrapper>()
                .AddSingleton<IZipperExecutor, ZipperExecutor>()
                .AddSingleton<ZipFileCommand>()
                .AddSingleton<ZipFolderCommand>()
                .AddSingleton<UnzipArchive>()
                .AddSingleton<Stopper>()
                .BuildServiceProvider();
}
