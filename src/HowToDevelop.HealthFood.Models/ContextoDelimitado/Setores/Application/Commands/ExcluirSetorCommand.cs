using HowToDevelop.Core.Comunicacao;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
{
    public class ExcluirSetorCommand : Command<int>
    {
        public ExcluirSetorCommand(in int id)
        {
            SetorId = id;
        }
        public int SetorId { get; }

    }
}
