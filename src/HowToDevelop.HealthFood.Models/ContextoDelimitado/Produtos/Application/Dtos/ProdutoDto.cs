namespace HowToDevelop.HealthFood.Produtos.Application.Dtos
{
    public class ProdutoDto
    {
        public int Id { get; set; }
        public string CodigoBarras { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public TipoProduto TipoProduto { get; set; }
    }
}
