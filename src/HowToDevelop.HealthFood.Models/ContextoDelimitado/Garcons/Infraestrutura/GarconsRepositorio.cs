using HowToDevelop.Core.Interfaces.Infraestrutura;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

namespace HowToDevelop.HealthFood.Infraestrutura.Garcons
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
    }
}
