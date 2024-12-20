using Microsoft.Extensions.DependencyInjection;
using Task.Homework13.Abstracts;
using Task.Homework13.Contracts;

namespace Task.Homework13.Implementation
{
    public class Mediator(IServiceProvider serviceProvider) : IMediator
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async System.Threading.Tasks.Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();
            foreach (var handler in handlers)
            {
                try
                {
                    await handler.Handle(notification, cancellationToken);
                }
                catch
                {
                    // ignored
                }
            }
        }

        public async Task<TResult> Send<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResult>
        {
            var handler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();

            return await handler.Handle(request, cancellationToken);
        }
    }
}