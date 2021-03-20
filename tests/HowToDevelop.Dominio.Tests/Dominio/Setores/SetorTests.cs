using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Infraestrutura.Setores;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;
using HowToDevelop.HealthFood.Setores;
using Shouldly;
using System.Linq;
using Xunit;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Dominio.Setores
{
    public class SetorTests
    {
        public SetorTests()
        { }

        [Fact(DisplayName = "Setor Válido Deve Ter Sucesso")]
        [Trait(nameof(Setor), "Criar")]
        public void Setor_Criar_DeveCriarComSucesso()
        {
            //Arrange & Act
            var setor = new SetorTestBuilder().Build();

            //Assert
            setor.IsSuccess.ShouldBeTrue();
        }


        [Fact(DisplayName = "Sem Nome Deve Falhar")]
        [Trait(nameof(Setor), "Criar")]
        public void Criar_SemNome_DeveFalhar()
        {
            //Arrange 
            var builder = new SetorTestBuilder()
                .ComNome("");

            //Act
            var result = builder.Build();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(NomeConstantes.NomeEhObrigatorio);
        }

        [Fact(DisplayName = "Sem Sigla Deve Falhar")]
        [Trait(nameof(Setor), "Criar")]
        public void Criar_SemSigla_DeveFalhar()
        {
            //Arrange 
            var builder = new SetorTestBuilder()
                .ComSigla("");

            //Act
            var result = builder.Build();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.SetorCampoSiglaObrigatorio);
        }

        [Fact(DisplayName = "Alterar Descricao Setor Válido Deve Ter Sucesso")]
        [Trait(nameof(Setor), nameof(Setor.AlterarDescricaoSetor))]
        public void Setor_AlterarDescricaoSetor_DeveCriarComSucesso()
        {
            //Arrange
            var setor = new SetorTestBuilder().Build().Value;
            string novoNome = "Área 51";
            string novaSigla = "A51";

            //Act
            Result result = setor.AlterarDescricaoSetor(novoNome, novaSigla);

            //Assert
            result.IsSuccess.ShouldBeTrue();
            setor.Nome.Valor.ShouldBe(novoNome);
            setor.Sigla.Valor.ShouldBe(novaSigla);
        }

        [Fact(DisplayName = "Alterar Descricao Setor Dados Inválidos Deve Falhar")]
        [Trait(nameof(Setor), nameof(Setor.AlterarDescricaoSetor))]
        public void AlterarDescricaoSetor_DadosInvalidos_DeveFalhar()
        {
            //Arrange
            var setor = new SetorTestBuilder().Build().Value;
            string novoNome = "Área 51";
            string novaSigla = "";

            //Act
            Result result = setor.AlterarDescricaoSetor(novoNome, novaSigla);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.SetorCampoSiglaObrigatorio);
            setor.Nome.Valor.ShouldNotBe(novoNome);
            setor.Sigla.Valor.ShouldNotBe(novaSigla);
        }

        [Fact(DisplayName = "Adicionar Nova Mesa Deve Ter Sucesso")]
        [Trait(nameof(Setor), nameof(Setor.AdicionarMesa))]
        public void AdicionarMesa_MesaNova_DeveAdicionarComSucesso()
        {
            //Arrange 
            Setor setor = new SetorTestBuilder().Build().Value;
            setor.AdicionarMesa(1);
            setor.AdicionarMesa(2);

            //Act
            ushort mesas = (ushort)(setor.Mesas.Max(n => n.Numeracao) + 1);
            Result result = setor.AdicionarMesa(mesas);

            //Assert
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Adicionar Mesa com numeracao existente Deve Falhar")]
        [Trait(nameof(Setor), nameof(Setor.AdicionarMesa))]
        public void AdicionarMesa_MesaExistente_DeveAdicionarComSucesso()
        {
            //Arrange 
            Setor setor = new SetorTestBuilder().Build().Value;
            setor.AdicionarMesa(1);
            setor.AdicionarMesa(2);

            //Act
            ushort mesas = setor.Mesas.Max(n => n.Numeracao);
            Result result = setor.AdicionarMesa(mesas);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.JaExisteUmaMesaComEstaNumeracaoParaSetor, setor.Id));
        }


        [Fact(DisplayName = "Remover Mesa Deve Ter Sucesso")]
        [Trait(nameof(Setor), nameof(Setor.RemoverMesa))]
        public void Setor_RemoverMesa_DeveRemoverComSucesso()
        {
            //Arrange 
            Setor setor = new SetorTestBuilder().Build().Value;
            setor.AdicionarMesa(1);
            setor.AdicionarMesa(2);

            //Act
            ushort mesa = setor.Mesas.Max(n => n.Numeracao);
            Result result = setor.RemoverMesa(mesa);

            //Assert
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Remover Mesa com numeracao inexistente Deve Falhar")]
        [Trait(nameof(Setor), nameof(Setor.RemoverMesa))]
        public void RemoverMesa_MesaInexistente_DeveFalhar()
        {
            //Arrange 
            Setor setor = new SetorTestBuilder().Build().Value;
            setor.AdicionarMesa(1);
            setor.AdicionarMesa(2);

            //Act
            ushort mesa = (ushort)(setor.Mesas.Max(n => n.Numeracao) + 1);
            Result result = setor.RemoverMesa(mesa);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.MesaInformadaNaoFoiLocalizada, mesa));
        }

    }

}
