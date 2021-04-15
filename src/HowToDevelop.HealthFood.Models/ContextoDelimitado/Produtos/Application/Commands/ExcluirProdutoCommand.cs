using HowToDevelop.Core.Comunicacao;

namespace HowToDevelop.HealthFood.Produtos.Application.Commands
{
    public class ExcluirProdutoCommand : Command<int, int>
    {
        public ExcluirProdutoCommand(in int id)
            : base(id)
        {

        }
    }
}
