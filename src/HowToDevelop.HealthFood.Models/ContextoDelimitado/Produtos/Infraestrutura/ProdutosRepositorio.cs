using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Produtos.Infraestrutura
{
    public class ProdutosRepositorio : IProdutosRepositorio
    {
        public IUnitOfWork UnitOfWork => _context;
        private readonly IDbConnection _connection;
        private readonly HealthFoodDbContext _context;

        public ProdutosRepositorio(HealthFoodDbContext context)
        {
            _context = context;
            _connection = _context.Database.GetDbConnection();
        }

        public async Task<Produto> ObterPorIdAsync(int id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public void Adicionar(Produto data)
        {
            _context.Produtos.Add(data);
        }

        public void Atualizar(Produto data)
        {
            _context.Produtos.Update(data);
        }

        public void Remover(Produto data)
        {
            _context.Produtos.Remove(data);
        }
    }
}
