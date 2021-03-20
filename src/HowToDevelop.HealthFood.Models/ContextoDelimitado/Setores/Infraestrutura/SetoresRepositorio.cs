using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.Setores;
using HowToDevelop.HealthFood.Setores;
using System.Collections.Generic;

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
    }
}
