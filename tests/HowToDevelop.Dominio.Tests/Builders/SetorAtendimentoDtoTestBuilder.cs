using HowToDevelop.HealthFood.Garcons.Application.Dtos;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;

namespace HowToDevelop.Dominio.Tests.Builders
{
    public class SetorAtendimentoDtoTestBuilder : ITestBuilder<SetorAtendimentoDto>
    {
        public SetorAtendimentoDtoTestBuilder()
        {
            Reiniciar();
        }

        public int Id { get; private set; }
        public SetorAtendimentoDtoTestBuilder ComId(int id)
        {
            Id = id;
            return this;
        }

        public int SetorId { get; private set; }
        public SetorAtendimentoDtoTestBuilder ComSetorId(int setorId)
        {
            SetorId = setorId;
            return this;
        }

        public string NomeSetor { get; private set; }
        public SetorAtendimentoDtoTestBuilder ComNomeSetor(string nomeSetor)
        {
            NomeSetor = nomeSetor;
            return this;
        }

        public SetorAtendimentoDto Build()
        {
            return new SetorAtendimentoDto
            {
                Id = Id,
                NomeSetor = NomeSetor,
                SetorId = SetorId
            };
        }

        public void Reiniciar()
        {
            Id = 1;
            SetorId = 1;
            NomeSetor = "Setor VIP";
        }
    }
}
