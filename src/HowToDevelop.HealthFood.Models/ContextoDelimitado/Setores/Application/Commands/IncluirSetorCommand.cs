using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Setores.Application.Dtos;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
{
    public class IncluirSetorCommand : Command<SetorDto>
    {
        public IncluirSetorCommand(in string nome, in string sigla)
        {
            Nome = nome;
            Sigla = sigla;
        }

        public string Nome { get; }
        public string Sigla { get; }
    }
}
