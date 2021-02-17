using HowToDevelop.Core;
using HowToDevelop.HealthFood.Dominio.Setores;

namespace HowToDevelop.HealthFood.Dominio.Garcons
{
    public sealed class SetorAtendimento: Entidade<int>
    {
        private SetorAtendimento()
        {

        }

        public SetorAtendimento(int id, Garcom garcom, Setor setor)
            :base(id)
        {
            _garcom = garcom;
            _setor = setor;
        }

        public SetorAtendimento(Garcom garcom, Setor setor)
        {
            _garcom = garcom;
            _setor = setor;
        }

        private Garcom _garcom;
        private Setor _setor;

        public Setor Setor => _setor;
        public Garcom Garcom => _garcom;
    }
}
