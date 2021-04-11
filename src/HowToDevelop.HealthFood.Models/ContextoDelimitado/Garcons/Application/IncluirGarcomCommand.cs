using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using System.Collections.Generic;

namespace HowToDevelop.HealthFood.Garcons.Application.Commands
{
    public class IncluirGarcomCommand : Command<IEnumerable<GarcomDto>>
    {
        public IncluirGarcomCommand(in string nome, in string apelido)
        {
            Nome = nome;
            Apelido = apelido;
        }
        public string Nome { get; private set; }
        public string Apelido { get; private set; }
    }
}
