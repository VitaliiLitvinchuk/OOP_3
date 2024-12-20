using Task.Homework13.Contracts;

namespace Task.Homework13.Abstracts;

public interface IPublisher
{
    System.Threading.Tasks.Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
        where TNotification : INotification;
}