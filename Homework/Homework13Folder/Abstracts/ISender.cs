using Task.Homework13.Contracts;

namespace Task.Homework13.Abstracts;

public interface ISender
{
    Task<TResult> Send<TRequest, TResult>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest<TResult>;
}