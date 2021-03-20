using HowToDevelop.Core.Comunicacao;

namespace HowToDevelop.HealthFood.Application.Setores
{
    public class IncluirSetorCommand :Comando<SetorDto>
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
