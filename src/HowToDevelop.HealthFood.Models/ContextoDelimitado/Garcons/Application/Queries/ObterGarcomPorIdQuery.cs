using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;

namespace HowToDevelop.HealthFood.Garcons.Application.Queries
{
    public class ObterGarcomPorIdQuery : IQuery<GarcomDto>
    {
        public ObterGarcomPorIdQuery(in int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
