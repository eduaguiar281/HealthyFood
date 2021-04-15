using CSharpFunctionalExtensions;
using HowToDevelop.Core.Interfaces.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using HowToDevelop.HealthFood.Setores.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura;

namespace HowToDevelop.HealthFood.Setores.Infraestrutura
{
    public class SetoresRepositorio : ISetoresRepositorio
    {
        private readonly string _sqlSetorInfo = @"SELECT s.Id,
                                                   s.Nome,
	                                               s.Sigla,
	                                               qm.QT as QuantidadeMesas,
	                                               CAST(CASE WHEN sa.QT = 0 THEN 0
	                                                         WHEN sa.QT > 0 THEN 1
	                                                    END as bit) as PossuiAtendente
                                              FROM Setores s
                                              OUTER APPLY (SELECT COUNT(m.Id) as QT 
	  		                                                 FROM Mesas m
			                                                WHERE m.SetorId = s.Id) as qm
                                              OUTER APPLY (SELECT COUNT(Id) as QT 
				                                             FROM SetoresAtendimento 
				                                            WHERE SetorId = s.Id) as sa";

        public IUnitOfWork UnitOfWork => _context;
        private readonly IDbConnection _connection;
        private readonly HealthFoodDbContext _context;

        public SetoresRepositorio(HealthFoodDbContext context)
        {
            _context = context;
            _connection = _context.Database.GetDbConnection();
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

        public async Task<IEnumerable<SetorInfoDto>> ObterTodosSetorInfoAsync()
        {
            return await _connection.QueryAsync<SetorInfoDto>(_sqlSetorInfo);
        }
    }
}
