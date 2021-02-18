using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Dominio.Garcons;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
{
    public class GarcomTestBuilder: ITestBuilder<Result<Garcom>>
    {
        public GarcomTestBuilder()
        {
            Reiniciar();
        }
        public string Nome { get; private set; }
        public string Apelido { get; private set; }

        public GarcomTestBuilder ComNome(string nome)
        {
            Nome = nome;
            return this;
        }

        public GarcomTestBuilder ComApelido(string apelido)
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
            Nome = "José da Silva";
            Apelido = "Garçom";
        }
    }
}
