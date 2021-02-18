using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Dominio.Setores;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
{
    public class MesaTestBuilder: ITestBuilder<Result<Mesa>>
    {
        public MesaTestBuilder()
        {
            Reiniciar();
        }

        public ushort Numeracao { get; protected set; }
        public void Reiniciar()
        {
            Numeracao = 1;
        }

        public MesaTestBuilder ComNumeracao(ushort numeracao)
        {
            Numeracao = numeracao;
            return this;
        }

        public Result<Mesa> Build()
        {
            return Mesa.Criar(Numeracao, 1);
        }
    }
}
