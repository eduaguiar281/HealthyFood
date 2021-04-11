using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Garcons.Application.Commands
{
    public class AlterarDadosPessoaisGarcomCommand : Command<IEnumerable<GarcomDto>, int>
    {
        public AlterarDadosPessoaisGarcomCommand(in int id, in string nome, in string apelido)
            :base(id)
        {
            Nome = nome;
            Apelido = apelido;
        }

        public string Nome { get; private set; }
        public string Apelido { get; private set; }

    }
}
