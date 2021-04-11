using CSharpFunctionalExtensions;
using MediatR;
using System;

namespace HowToDevelop.Core.Comunicacao.Interfaces
{
    public interface ICommand<T, TKey> : IRequest<Result<T>>
    {
        DateTime Timestamp { get; }

        TKey RaizAgregacaoId { get; }
    }
    public interface ICommand<T> : IRequest<Result<T>>
    {
        DateTime Timestamp { get; }
    }

}
