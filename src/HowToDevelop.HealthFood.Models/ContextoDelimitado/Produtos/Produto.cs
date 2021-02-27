using CSharpFunctionalExtensions;
using HowToDevelop.Core;
using HowToDevelop.Core.Entidade;
using HowToDevelop.Core.Interfaces;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.Core.ValidacoesPadrao;
using System.Diagnostics.CodeAnalysis;

namespace HowToDevelop.HealthFood.Dominio.Produtos
{
    public sealed class Produto: RaizAgregacao<int>
    {
        [ExcludeFromCodeCoverage]
        private Produto()
        {
            
        }

        private Produto(in string codigoBarras, 
            in string descricao, 
            in Preco preco, 
            TipoProduto tipo, 
            int id)
            :base(id)
        {
            CodigoBarras = codigoBarras;
            Descricao = descricao;
            Preco = preco;
            TipoProduto = tipo;
        }

        public string CodigoBarras { get; private set; }
        public string Descricao { get; private set; }
        public Preco Preco { get; private set; }
        public TipoProduto TipoProduto { get; }

        public Result AlterarDados(in string codigoBarras, in string descricao, in Preco preco)
        {
            var (_, isFailure, error) = ValidarDadosProduto(codigoBarras, descricao, preco);
            
            if (isFailure)
            {
                return Result.Failure(error);
            }

            CodigoBarras = codigoBarras;
            Descricao = descricao;
            Preco = preco;

            return Result.Success();
        }

        private static Result ValidarDadosProduto(in string codigoBarras, in string descricao, in Preco preco)
        {
            return Result.Combine(
                            codigoBarras.NaoDeveSerNuloOuVazio(ProdutosConstantes.ProdutoCodigoBarrasEhObrigatorio),
                            codigoBarras.TamanhoMenorOuIgual(ProdutosConstantes.ProdutoTamanhoCampoCodigoBarras, ProdutosConstantes.ProdutoCodigoBarrasDeveTerNoMaximoNCaracteres),
                            descricao.NaoDeveSerNuloOuVazio(ProdutosConstantes.ProdutoDescricaoEhObrigatorio),
                            descricao.TamanhoMenorOuIgual(ProdutosConstantes.ProdutoTamanhoCampoDescricao, ProdutosConstantes.ProdutoDescricaoDeveTerNoMaximoNCaracteres),
                            preco.NaoDeveSerNulo(ProdutosConstantes.ProdutoPrecoNaoFoiInformado));
        }

        public static Result<Produto> CriarTipoBebida(in string codigoBarras, in string descricao, in Preco preco, in int id = 0)
        {
            var (_, isFailure, error) = ValidarDadosProduto(codigoBarras, descricao, preco);

            if (isFailure)
            {
                return Result.Failure<Produto>(error);
            }

            return Result.Success(new Produto(codigoBarras, descricao, preco, TipoProduto.Bebida, id));
        }

        public static Result<Produto> CriarTipoLanche(in string codigoBarras, in string descricao, in Preco preco, in int id = 0)
        {
            var (_, isFailure, error) = ValidarDadosProduto(codigoBarras, descricao, preco);

            if (isFailure)
            {
                return Result.Failure<Produto>(error);
            }

            return Result.Success(new Produto(codigoBarras, descricao, preco, TipoProduto.Lanche, id));
        }

        public static Result<Produto> CriarTipoOutros(in string codigoBarras, in string descricao, in Preco preco, in int id = 0)
        {
            var (_, isFailure, error) = ValidarDadosProduto(codigoBarras, descricao, preco);

            if (isFailure)
            {
                return Result.Failure<Produto>(error);
            }

            return Result.Success(new Produto(codigoBarras, descricao, preco, TipoProduto.Outros, id));
        }
    }
}
