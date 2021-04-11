using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Setores.Application.Dtos;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
{
    public class AlterarDescricaoSetorCommand : Command<SetorDto, int>
    {
        public AlterarDescricaoSetorCommand(in int id, in string nome, in string sigla)
            :base(id)
        {
            Nome = nome;
            Sigla = sigla;
        }

        public string Nome { get; }
        public string Sigla { get; }
    }
}
