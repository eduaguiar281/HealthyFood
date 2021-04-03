using CSharpFunctionalExtensions;
using MediatR;

namespace HowToDevelop.Core.Comunicacao.Interfaces
{
    public interface IQuery<T> : IRequest<Result<T>>
    {
    }

}
