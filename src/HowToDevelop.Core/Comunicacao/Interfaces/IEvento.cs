using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.Core.Comunicacao.Interfaces
{
    public interface IEvento: INotification
    {
        DateTime Timestamp { get; }
    }
}
