using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Setores.Application.Dtos;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
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
