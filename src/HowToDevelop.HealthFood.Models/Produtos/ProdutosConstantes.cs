using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Produtos
{
    public static class ProdutosConstantes
    {
        public const string ProdutoCodigoBarrasEhObrigatorio = "Campo Código de Barras é Obrigatório!";

        public const string ProdutoDescricaoEhObrigatorio = "Campo Descrição do Produto é Obrigatório!";

        public const int ProdutoTamanhoCampoDescricao = 70;

        public const int ProdutoTamanhoCampoCodigoBarras = 30;

        public static readonly string ProdutoDescricaoDeveTerNoMaximoNCaracteres = $"Campo Descrição deve ter no máximo {ProdutoTamanhoCampoDescricao} caracteres!";

        public static readonly string ProdutoCodigoBarrasDeveTerNoMaximoNCaracteres = $"Campo Código de Barras deve ter no máximo {ProdutoTamanhoCampoCodigoBarras} caracteres!";

        public const string ProdutoPrecoNaoPodeSerMenorOuIgualZero = "Campo preço não pode ser inferior ou igual a zero!";
    }
}
