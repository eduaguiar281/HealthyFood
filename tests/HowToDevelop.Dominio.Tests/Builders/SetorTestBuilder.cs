using HowToDevelop.HealthFood.Dominio.Setores;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
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
            Id = 1;
        }

        public string Nome { get; protected set; }
        public string Sigla { get; protected set; }
        public int Id { get; protected set; }
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
        public SetorTestBuilder ComId(int id)
        {
            Id = id;
            return this;
        }
        public Setor Build()
        {
            var setor = new Setor(Id)
            {
                Nome = this.Nome,
                Sigla = this.Sigla
            };

            setor.AdicionarMesa(1);
            setor.AdicionarMesa(2);
            setor.AdicionarMesa(3);
            return setor;
        }
    }
}
