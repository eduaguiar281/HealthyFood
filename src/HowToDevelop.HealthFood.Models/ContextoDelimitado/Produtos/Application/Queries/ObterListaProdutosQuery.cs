using CSharpFunctionalExtensions;
using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.Core.Enums;
using HowToDevelop.Core.Infraestrutura;
using HowToDevelop.HealthFood.Produtos.Application.Dtos;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Produtos.Application.Queries
{
    public enum OrdemProdutosQuery
    {
        Id,
        Descricao,
        CodigoBarras,
        Preco
    }

    public class ObterListaProdutosQuery : IQuery<PagedList<ProdutoDto>>
    {
        public ObterListaProdutosQuery(in string termoBusca, 
            in Dictionary<OrdemProdutosQuery, TipoOrdemQuery> ordem, 
            in TipoProduto tipo)
        {
            TermoBusca = termoBusca;
            Ordem = ordem;
            Tipo = tipo;
        }
        public string TermoBusca { get; }
        public Dictionary<OrdemProdutosQuery, TipoOrdemQuery> Ordem { get; }
        public Maybe<TipoProduto> Tipo { get; }
    }
}
