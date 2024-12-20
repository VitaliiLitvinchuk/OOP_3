using Task.Homework13.Contracts;

namespace Task.Homework13.Abstracts
{
    public interface IRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public interface IRequestHandler<in TRequest>
        where TRequest : IRequest
    {
        System.Threading.Tasks.Task Handle(TRequest request, CancellationToken cancellationToken);
    }
}