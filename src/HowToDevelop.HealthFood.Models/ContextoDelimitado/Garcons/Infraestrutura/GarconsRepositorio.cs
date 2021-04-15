using CSharpFunctionalExtensions;
using Dapper;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Garcons.Infraestrutura
{
    public class GarconsRepositorio : IGarconsRepositorio
    {
        public IUnitOfWork UnitOfWork => _context;
        private readonly IDbConnection _connection;
        private readonly HealthFoodDbContext _context;

        public GarconsRepositorio(HealthFoodDbContext context)
        {
            _context = context;
            _connection = _context.Database.GetDbConnection();
        }

        public void Adicionar(Garcom data)
        {
            _context.Garcons.Add(data);
        }

        public void Atualizar(Garcom data)
        {
            _context.Garcons.Update(data);
        }

        public void Remover(Garcom data)
        {
            _context.Garcons.Remove(data);
        }

        public async Task<Maybe<Garcom>> ObterPorIdAsync(int id)
        {
            return await _context.Garcons.FindAsync(id);
        }

        public async Task<Maybe<Garcom>> ObterComSetoresAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Garcons
                .Include(g => g.SetoresAtendimento)
                .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<SetorAtendimentoDto>> ObterListaSetoresAsync(int garcomId)
        {
            string sql = @"Select sa.Id, sa.SetorId, s.Nome as NomeSetor
                             FROM SetoresAtendimento sa
                            INNER JOIN Setores s on s.Id = sa.SetorId
                            WHERE sa.GarcomId = @garcomId";

            var parameters = new DynamicParameters();
            parameters.Add("@garcomId", garcomId);

            return await _connection.QueryAsync<SetorAtendimentoDto>(sql, parameters);
        }


        public async Task<bool> SetorExiste(int setorId)
        {
            string sql = @"Select COUNT(s.Id) 
                             FROM Setores s
                            WHERE s.Id = @setorId";
            var parameters = new DynamicParameters();
            parameters.Add("@setorId", setorId);

            return await _connection.QueryFirstOrDefaultAsync<int>(sql, parameters) > 0;
        }

        public async Task<IEnumerable<GarcomInfoDto>> ObterTodosGarconsAsync()
        {
            string sql = @"SELECT g.Id, g.Nome, g.Apelido, sa.QuantidadeSetores 
                             FROM Garcons g
                             OUTER APPLY (SELECT COUNT(id) as QuantidadeSetores
                                            FROM SetoresAtendimento s
				                        WHERE s.GarcomId = g.Id) sa";
            
            return await _connection.QueryAsync<GarcomInfoDto>(sql);
        }
    }
}
