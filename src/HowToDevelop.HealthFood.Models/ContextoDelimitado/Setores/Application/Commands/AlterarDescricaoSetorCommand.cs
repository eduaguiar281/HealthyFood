using HowToDevelop.Core.Comunicacao;
using HowToDevelop.HealthFood.Setores.Application.Dtos;

namespace HowToDevelop.HealthFood.Setores.Application.Commands
{
    public class AlterarDescricaoSetorCommand : Command<SetorDto>
    {
        public AlterarDescricaoSetorCommand(in int id, in string nome, in string sigla)
        {
            Id = id;
            Nome = nome;
            Sigla = sigla;
        }

        public int Id { get; }
        public string Nome { get; }
        public string Sigla { get; }
    }
}
