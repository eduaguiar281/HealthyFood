using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Setores.Application.Dtos;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
{
    public class RemoverMesaSetorCommand : Command<IEnumerable<MesaDto>, int>
    {
        public RemoverMesaSetorCommand(in int setorId, in ushort numeracao)
            :base(setorId)
        {
            Numeracao = numeracao;
        }

        public ushort Numeracao { get; }
    }
}
