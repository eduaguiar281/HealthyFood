using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Setores;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Builders
{
    public class SetorTestBuilder
    {
        public SetorTestBuilder()
        {
            Reiniciar();
        }

        public void Reiniciar()
        {
            Nome = "Setor 01";
            Sigla = "ST1";
        }

        public string Nome { get; protected set; }
        public string Sigla { get; protected set; }

        public SetorTestBuilder ComNome(string nome)
        {
            Nome = nome;
            return this;
        }
        public SetorTestBuilder ComSigla(string sigla)
        {
            Sigla = sigla;
            return this;
        }

        public Result<Setor> Build()
        {
            return Setor.Criar(Nome, Sigla);
        }
    }
}
