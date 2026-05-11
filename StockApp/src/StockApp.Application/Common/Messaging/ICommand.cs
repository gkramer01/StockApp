using MediatR;

namespace StockApp.Application.Common.Messaging;

public interface ICommand<TResponse> : IRequest<TResponse>
{
}
