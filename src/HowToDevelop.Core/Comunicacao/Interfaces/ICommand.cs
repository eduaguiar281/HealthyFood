using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.Core.Comunicacao.Interfaces
{
    public interface ICommand<T>: IRequest<Result<T>>
    {
        DateTime Timestamp { get; }
    }
}
