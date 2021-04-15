namespace HowToDevelop.HealthFood.Produtos
{
    public static class ProdutosConstantes
    {
        public const string ProdutoCodigoBarrasEhObrigatorio = "Campo Código de Barras é Obrigatório!";

        public const int ProdutoTamanhoCampoCodigoBarras = 30;

        public static readonly string ProdutoCodigoBarrasDeveTerNoMaximoNCaracteres = $"Campo Código de Barras deve ter no máximo {ProdutoTamanhoCampoCodigoBarras} caracteres!";

        public const string ProdutoPrecoNaoFoiInformado = "Campo preço não foi informado!";
    }
}
