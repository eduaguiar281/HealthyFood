using AutoMapper;
using HowToDevelop.HealthFood.Application.Setores;
using HowToDevelop.HealthFood.Setores;

namespace HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions
{
    public class DomainModelToDtoMappingProfile : Profile
    {
        public DomainModelToDtoMappingProfile()
        {
            CreateMap<Setor, SetorDto>();
        }
    }
}
