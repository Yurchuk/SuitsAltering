using MediatR;

namespace SuitsAltering.BL.Abstractions
{
    public interface IDomainQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
