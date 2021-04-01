using HowToDevelop.Core.Comunicacao;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Application.Setores
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
