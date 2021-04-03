using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.HealthFood.Setores.Application.Dtos;

namespace HowToDevelop.HealthFood.Setores.Application.Queries
{
    public class ObterSetorPorIdQuery : IQuery<SetorDto>
    {
        public ObterSetorPorIdQuery(in int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
