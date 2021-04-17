using AutoMapper;
using HowToDevelop.HealthFood.Garcons;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Produtos;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;
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
            CreateMap<Produto, ProdutoDto>();
        }
    }
}
