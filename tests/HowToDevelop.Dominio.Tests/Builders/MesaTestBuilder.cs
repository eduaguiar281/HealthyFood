using HowToDevelop.HealthFood.Dominio.Setores;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
{
    public class MesaTestBuilder: ITestBuilder<Mesa>
    {
        public MesaTestBuilder()
        {
            Reiniciar();
        }

        public int Id { get; protected set; }

        public ushort Numeracao { get; protected set; }
        public void Reiniciar()
        {
            Numeracao = 1;
            Id = 1;
        }

        public MesaTestBuilder ComNumeracao(ushort numeracao)
        {
            Numeracao = numeracao;
            return this;
        }

        public MesaTestBuilder ComId(int id)
        {
            Id = id;
            return this;
        }


        public Mesa Build()
        {
            return new Mesa(Id)
            {
                Numeracao = this.Numeracao
            };
        }
    }
}
