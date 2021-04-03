using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Setores.Application.Dtos;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
{
    public class RemoverMesaSetorCommand : Command<IEnumerable<MesaDto>>
    {
        public RemoverMesaSetorCommand(in int setorId, in ushort numeracao)
        {
            SetorId = setorId;
            Numeracao = numeracao;
        }

        public int SetorId { get; }
        public ushort Numeracao { get; }

    }
}
