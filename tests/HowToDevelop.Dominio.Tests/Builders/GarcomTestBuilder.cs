using HowToDevelop.HealthFood.Dominio.Garcons;
using HowToDevelop.HealthFood.Dominio.Setores;
using System;
using System.Collections.Generic;
using System.Text;

namespace HowToDevelop.HealthFood.Dominio.Tests.Builders
{
    public class GarcomTestBuilder: ITestBuilder<Garcom>
    {
        public GarcomTestBuilder()
        {
            Reiniciar();
        }
        public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Apelido { get; private set; }

        private IEnumerable<Setor> _setores;
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
        public GarcomTestBuilder ComSetores(IEnumerable<Setor> setores)
        {
            _setores = setores;
            return this;
        }
        public Garcom Build()
        {
            var garcom = new Garcom(Id)
            {
                Apelido = this.Apelido,
                Nome = this.Nome                
            };
            
            foreach (var setor in _setores)
                garcom.AdicionarSetor(setor);

            return garcom;
        }
        public void Reiniciar()
        {
            Nome = "José da Silva";
            Apelido = "Garçom";
            _setores = new List<Setor>() { new SetorTestBuilder().Build() };
        }
    }
}
