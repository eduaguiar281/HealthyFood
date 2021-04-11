using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Setores.Application.Dtos;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
{
    public class AdicionarMesaSetorCommand : Command<IEnumerable<MesaDto>, int>
    {
        public AdicionarMesaSetorCommand(in int setorId, in ushort numeracao)
            :base(setorId)
        {
            Numeracao = numeracao;
        }
        public ushort Numeracao { get; }
    }
}
