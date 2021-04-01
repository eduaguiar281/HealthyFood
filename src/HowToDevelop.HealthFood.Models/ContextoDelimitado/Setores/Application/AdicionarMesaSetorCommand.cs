using HowToDevelop.Core.Comunicacao;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Application.Setores
{
    public class AdicionarMesaSetorCommand : Command<IEnumerable<MesaDto>>
    {
        public AdicionarMesaSetorCommand(in int setorId, in ushort numeracao)
        {
            SetorId = setorId;
            Numeracao = numeracao;
        }
        public int SetorId { get; }
        public ushort Numeracao { get; }
    }
}
