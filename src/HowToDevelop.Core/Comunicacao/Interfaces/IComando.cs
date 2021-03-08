using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.Core.Comunicacao.Interfaces
{
    public interface IComando<T>: IRequest<Result<T>>
    {
        DateTime Timestamp { get; }
    }
}
