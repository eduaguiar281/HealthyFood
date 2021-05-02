using CSharpFunctionalExtensions;
using HowToDevelop.Core.Infraestrutura;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;
using HowToDevelop.HealthFood.Produtos;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;
using HowToDevelop.HealthFood.Produtos.Application.Queries;
using HowToDevelop.HealthFood.Produtos.Infraestrutura;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Dominio.ContextoDelimitado.Produtos.Application.Queries
{
    public class ProdutosQueryHandler : IRequestHandler<ObterProdutoPorIdQuery, Result<ProdutoDto>>,
        IRequestHandler<ObterListaProdutosQuery, Result<PagedList<ProdutoDto>>>
    {

        private readonly IProdutosRepositorio _repository;
        public ProdutosQueryHandler(IProdutosRepositorio repositorio)
        {
            _repository = repositorio;
        }


        public async Task<Result<ProdutoDto>> Handle(ObterProdutoPorIdQuery request, CancellationToken cancellationToken)
        {
            Maybe<Produto> produto = await _repository.ObterPorIdAsync(request.Id);
            if (produto.HasNoValue)
            {
                return Result.Failure<ProdutoDto>(string.Format(ProdutosConstantes.ProdutoNaoFoiEncontradoComIdInformado, request.Id));
            }
            return Result.Success(produto.Value.Map<ProdutoDto>());
        }

        public async Task<Result<PagedList<ProdutoDto>>> Handle(ObterListaProdutosQuery request, CancellationToken cancellationToken)
        {
            

            throw new NotImplementedException();
        }
    }
}
