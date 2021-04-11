using HowToDevelop.Core.Comunicacao;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
{
    public class ExcluirSetorCommand : Command<int, int>
    {
        public ExcluirSetorCommand(in int id)
            :base(id)
        {
        }
    }
}
