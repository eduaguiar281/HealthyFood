using CSharpFunctionalExtensions;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.Setores;
using HowToDevelop.HealthFood.Setores;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Dominio.ContextoDelimitado.Setores.Infraestrutura
{
    public class SetoresRepositorio : ISetoresRepositorio
    {

        public IUnitOfWork UnitOfWork => _context;
        private readonly HealthFoodDbContext _context;

        public SetoresRepositorio(HealthFoodDbContext context)
        {
            _context = context;
        }

        public void Adicionar(Setor data)
        {
            _context.Setores.Add(data);
        }

        public void Atualizar(Setor data)
        {
            _context.Setores.Update(data);
        }

        public void Remover(Setor data)
        {
            _context.Setores.Remove(data);
        }

        public async Task<Maybe<Setor>> ObterPorIdAsync(int id)
        {
            return await _context.Setores.FindAsync(id);
        }

        public async Task<Maybe<Setor>> ObterComMesasPorIdAsync(int id)
        {
            return await _context.Setores.Include(s=> s.Mesas).FirstOrDefaultAsync(s=> s.Id == id);
        }
    }
}
