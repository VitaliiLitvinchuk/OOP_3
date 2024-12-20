namespace Task.Homework13.Contracts;

public interface IRequest : IBaseRequest { }

public interface IRequest<out TResponse> : IBaseRequest { }

public interface IBaseRequest { }
