using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Dominio.Setores;
using HowToDevelop.HealthFood.Dominio.Tests.Builders;
using Shouldly;
using System.Linq;
using Xunit;

namespace HowToDevelop.HealthFood.Dominio.Tests.Dominio.Setores
{
    public class SetorTests
    {
        public SetorTests()
        { }

        [Fact(DisplayName = "Setor Válido Deve Ter Sucesso")]
        [Trait(nameof(Setor), "Validar")]
        public void Setor_Criar_DeveCriarComSucesso()
        {
            //Arrange & Act
            var setor = new SetorTestBuilder().Build();

            //Assert
            setor.EhValido().IsSuccess.ShouldBeTrue();
        }


        [Fact(DisplayName = "Sem Nome Deve Falhar")]
        [Trait(nameof(Setor), "Validar")]
        public void Validar_SemNome_DeveFalhar()
        {
            //Arrange 
            var builder = new SetorTestBuilder()
                .ComNome("");

            //Act
            var result = builder.Build().EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.SetorCampoNomeObrigatorio);
        }

        [Fact(DisplayName = "Nome Acima Limite Caracteres Deve Falhar")]
        [Trait(nameof(Setor), "Validar")]
        public void Validar_NomeAcimeLimiteCaracteres_DeveFalhar()
        {
            //Arrange 
            var builder = new SetorTestBuilder()
                .ComNome("Setor 01".PadRight(SetoresConstantes.SetorTamanhoMaximoNome + 5));

            //Act
            var result = builder.Build().EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.SetorCampoNomeDeveTerAteNCaracteres);
        }

        [Fact(DisplayName = "Sem Sigla Deve Falhar")]
        [Trait(nameof(Setor), "Validar")]
        public void Validar_SemSigla_DeveFalhar()
        {
            //Arrange 
            var builder = new SetorTestBuilder()
                .ComSigla("");

            //Act
            var result = builder.Build().EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.SetorCampoSiglaObrigatorio);
        }

        [Fact(DisplayName = "Sigla Acima Limite Caracteres Deve Falhar")]
        [Trait(nameof(Setor), "Validar")]
        public void Validar_SiglaAcimeLimiteCaracteres_DeveFalhar()
        {
            //Arrange 
            var builder = new SetorTestBuilder()
                .ComSigla("ST1".PadRight(SetoresConstantes.SetorTamanhoMaximoSigla + 2));

            //Act
            var result = builder.Build().EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.SetorCampoSiglaDeveTerAteNCaracteres);
        }


        [Fact(DisplayName = "Adicionar Nova Mesa Deve Ter Sucesso")]
        [Trait(nameof(Setor), nameof(Setor.AdicionarMesa))]
        public void AdicionarMesa_MesaNova_DeveAdicionarComSucesso()
        {
            //Arrange 
            Setor setor = new SetorTestBuilder().Build();

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
            Setor setor = new SetorTestBuilder().Build();

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
            Setor setor = new SetorTestBuilder().Build();

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
            Setor setor = new SetorTestBuilder().Build();

            //Act
            ushort mesa = (ushort)(setor.Mesas.Max(n => n.Numeracao) + 1);
            Result result = setor.RemoverMesa(mesa);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(SetoresConstantes.MesaInformadaNaoFoiLocalizada, mesa));
        }

    }

}
