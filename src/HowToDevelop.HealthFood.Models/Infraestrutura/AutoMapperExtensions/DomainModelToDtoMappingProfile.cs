using AutoMapper;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura.Garcons;
using HowToDevelop.HealthFood.Setores;
using HowToDevelop.HealthFood.Setores.Application.Dtos;

namespace HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions
{
    public class DomainModelToDtoMappingProfile : Profile
    {
        public DomainModelToDtoMappingProfile()
        {
            CreateMap<Setor, SetorDto>();
            CreateMap<Mesa, MesaDto>();
            CreateMap<Garcom, GarcomDto>();
        }
    }
}
