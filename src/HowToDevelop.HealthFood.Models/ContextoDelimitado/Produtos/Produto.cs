using CSharpFunctionalExtensions;
using HowToDevelop.Core.Entidade;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ValidacoesPadrao;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Produtos
{
    public sealed class Produto : RaizAgregacao
    {
        [ExcludeFromCodeCoverage]
        private Produto()
        {

        }

        private Produto(in string codigoBarras,
            in Descricao descricao,
            in Preco preco,
            TipoProduto tipo,
            int id)
            : base(id)
        {
            CodigoBarras = codigoBarras;
            Descricao = descricao;
            Preco = preco;
            TipoProduto = tipo;
        }

        public string CodigoBarras { get; private set; }
        public Descricao Descricao { get; private set; }
        public Preco Preco { get; private set; }
        public TipoProduto TipoProduto { get; }

        public Result AlterarDados(in string codigoBarras, in string descricao, in Preco preco)
        {
            Result<Descricao> descricaoResult = Descricao.Criar(descricao);
            var (_, isFailure, error) = Result.Combine(ValidarDadosProduto(codigoBarras, preco), descricaoResult);

            if (isFailure)
            {
                return Result.Failure(error);
            }

            CodigoBarras = codigoBarras;
            Descricao = descricaoResult.Value;
            Preco = preco;

            return Result.Success();
        }

        private static Result ValidarDadosProduto(in string codigoBarras, in Preco preco)
        {
            return Result.Combine(
                            codigoBarras.NaoDeveSerNuloOuVazio(ProdutosConstantes.ProdutoCodigoBarrasEhObrigatorio),
                            codigoBarras.TamanhoMenorOuIgual(ProdutosConstantes.ProdutoTamanhoCampoCodigoBarras, ProdutosConstantes.ProdutoCodigoBarrasDeveTerNoMaximoNCaracteres),
                            preco.NaoDeveSerNulo(ProdutosConstantes.ProdutoPrecoNaoFoiInformado));
        }

        public static Result<Produto> CriarTipoBebida(in string codigoBarras, in string descricao, in Preco preco, in int id = 0)
        {
            Result<Descricao> descricaoResult = Descricao.Criar(descricao);
            var (_, isFailure, error) = Result.Combine(ValidarDadosProduto(codigoBarras, preco), descricaoResult);

            if (isFailure)
            {
                return Result.Failure<Produto>(error);
            }

            return Result.Success(new Produto(codigoBarras, descricaoResult.Value, preco, TipoProduto.Bebida, id));
        }

        public static Result<Produto> CriarTipoLanche(in string codigoBarras, in string descricao, in Preco preco, in int id = 0)
        {
            Result<Descricao> descricaoResult = Descricao.Criar(descricao);
            var (_, isFailure, error) = Result.Combine(ValidarDadosProduto(codigoBarras, preco), descricaoResult);

            if (isFailure)
            {
                return Result.Failure<Produto>(error);
            }

            return Result.Success(new Produto(codigoBarras, descricaoResult.Value, preco, TipoProduto.Lanche, id));
        }

        public static Result<Produto> CriarTipoOutros(in string codigoBarras, in string descricao, in Preco preco, in int id = 0)
        {
            Result<Descricao> descricaoResult = Descricao.Criar(descricao);
            var (_, isFailure, error) = Result.Combine(ValidarDadosProduto(codigoBarras, preco), descricaoResult);

            if (isFailure)
            {
                return Result.Failure<Produto>(error);
            }

            return Result.Success(new Produto(codigoBarras, descricaoResult.Value, preco, TipoProduto.Outros, id));
        }
    }
}
