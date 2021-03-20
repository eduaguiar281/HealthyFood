using System;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Interfaces.Infraestrutura
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
    }
}
