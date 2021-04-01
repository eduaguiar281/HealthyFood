using HowToDevelop.Core.Comunicacao;

namespace HowToDevelop.HealthFood.Application.Setores
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
