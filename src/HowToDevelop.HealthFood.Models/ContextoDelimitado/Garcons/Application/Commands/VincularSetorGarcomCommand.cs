using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Garcons.Application.Commands
{
    public class VincularSetorGarcomCommand : Command<IEnumerable<SetorAtendimentoDto>, int>
    {
        public VincularSetorGarcomCommand(in int id, in int setorId)
            :base(id)
        {
            SetorId = setorId;
        }

        public int SetorId { get; private set; }
    }
}
