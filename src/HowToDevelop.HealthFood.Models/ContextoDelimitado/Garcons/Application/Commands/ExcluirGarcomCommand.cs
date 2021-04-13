using HowToDevelop.Core.Comunicacao;

namespace HowToDevelop.HealthFood.Garcons.Application.Commands
{
    public class ExcluirGarcomCommand : Command<int, int>
    {
        public ExcluirGarcomCommand(in int id)
            : base(id)
        {

        }
    }
}
