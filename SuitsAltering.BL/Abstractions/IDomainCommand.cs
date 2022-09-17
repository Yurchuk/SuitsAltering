using MediatR;

namespace SuitsAltering.BL.Abstractions
{
    public interface IDomainCommand<out TResponse> : IRequest<TResponse>
    {
    }

    public interface IDomainCommand : IRequest
    {
    }
}
