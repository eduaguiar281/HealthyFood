using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Garcons;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Builders
{
    public class GarcomTestBuilder : ITestBuilder<Result<Garcom>>
    {
        public GarcomTestBuilder()
        {
            Reiniciar();
        }
        public Nome Nome { get; private set; }
        public Apelido Apelido { get; private set; }

        public GarcomTestBuilder ComNome(Nome nome)
        {
            Nome = nome;
            return this;
        }

        public GarcomTestBuilder ComApelido(Apelido apelido)
        {
            Apelido = apelido;
            return this;
        }

        public Result<Garcom> Build()
        {
            return Garcom.Criar(Nome, Apelido, 1);
        }

        public void Reiniciar()
        {
            Nome = Nome.Criar("José da Silva").Value;
            Apelido = Apelido.Criar("Garçom").Value;
        }
    }
}
