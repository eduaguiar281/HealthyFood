using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;
using HowToDevelop.HealthFood.Produtos.Infraestrutura;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using HowToDevelop.HealthFood.Infraestrutura.AutoMapperExtensions;

namespace HowToDevelop.HealthFood.Produtos.Application.Commands
{
    public class ProdutosCommandHandler : IRequestHandler<IncluirProdutoCommand, Result<ProdutoDto>>,
        IRequestHandler<AlterarDadosProdutoCommand, Result<ProdutoDto>>,
        IRequestHandler<ExcluirProdutoCommand, Result<int>>
    {
        private readonly IProdutosRepositorio _repositorio;
        public ProdutosCommandHandler(IProdutosRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        private Result<Produto> ResolveCriacaoProduto(IncluirProdutoCommand request)
        {
            switch (request.TipoProduto)
            {
                case TipoProduto.Bebida:
                    return Produto.CriarTipoBebida(request.CodigoBarras, request.Descricao, new Preco(request.Preco));
                case TipoProduto.Lanche:
                    return Produto.CriarTipoLanche(request.CodigoBarras, request.Descricao, new Preco(request.Preco));
                case TipoProduto.Outros:
                default:
                    return Produto.CriarTipoOutros(request.CodigoBarras, request.Descricao, new Preco(request.Preco));
            }
        }

        public async Task<Result<ProdutoDto>> Handle(IncluirProdutoCommand request, CancellationToken cancellationToken)
        {
            var (_, isFailure, produto, erro) = ResolveCriacaoProduto(request);
            if (isFailure)
            {
                return Result.Failure<ProdutoDto>(erro);
            }
            _repositorio.Adicionar(produto);
            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);
            if (result <= 0)
            {
                return Result.Failure<ProdutoDto>(ProdutosConstantes.ProdutoNaoFoiPossivelSalvar);
            }
            return Result.Success(produto.Map<ProdutoDto>());
        }

        public async Task<Result<ProdutoDto>> Handle(AlterarDadosProdutoCommand request, CancellationToken cancellationToken)
        {
            Maybe<Produto> produto = await _repositorio.ObterPorIdAsync(request.RaizAgregacaoId);

            if (produto.HasNoValue)
            {
                return Result.Failure<ProdutoDto>(string.Format(ProdutosConstantes.ProdutoNaoFoiEncontradoComIdInformado, request.RaizAgregacaoId));
            }

            var (_, isFailure, erro) = produto.Value.AlterarDados(request.CodigoBarras, request.Descricao, new Preco(request.Preco));

            if (isFailure)
            {
                return Result.Failure<ProdutoDto>(erro);
            }

            _repositorio.Atualizar(produto.Value);

            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);

            if (result <= 0)
            {
                return Result.Failure<ProdutoDto>(ProdutosConstantes.ProdutoNaoFoiPossivelSalvar);
            }

            return Result.Success(produto.Value.Map<ProdutoDto>());
        }

        public async Task<Result<int>> Handle(ExcluirProdutoCommand request, CancellationToken cancellationToken)
        {
            Maybe<Produto> produto = await _repositorio.ObterPorIdAsync(request.RaizAgregacaoId);

            if (produto.HasNoValue)
            {
                return Result.Failure<int>(string.Format(ProdutosConstantes.ProdutoNaoFoiEncontradoComIdInformado, request.RaizAgregacaoId));
            }

            _repositorio.Remover(produto.Value);

            int result = await _repositorio.UnitOfWork.CommitAsync(cancellationToken);

            if (result <= 0)
            {
                return Result.Failure<int>(ProdutosConstantes.ProdutoNaoFoiPossivelExcluir);
            }

            return Result.Success(request.RaizAgregacaoId);
        }
    }
}
