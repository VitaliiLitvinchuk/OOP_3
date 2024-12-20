using System;
using Task.Homework13.Contracts;

namespace Task.Homework13.Abstracts
{
    public interface INotificationHandler<TNotification>
        where TNotification : INotification
    {
        System.Threading.Tasks.Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}