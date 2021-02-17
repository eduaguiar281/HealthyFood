using CSharpFunctionalExtensions;
using HowToDevelop.HealthFood.Dominio.Garcons;
using HowToDevelop.HealthFood.Dominio.Setores;
using HowToDevelop.HealthFood.Dominio.Tests.Builders;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace HowToDevelop.HealthFood.Dominio.Tests.Dominio.Garcons
{
    public class GarcomTests
    {
        [Fact(DisplayName = "Válido Deve Ter Sucesso")]
        [Trait(nameof(Garcom), "Validar")]
        public void Garcom_Validar_DeveTerSucesso()
        {
            //Arrange & Act
            var garcom = new GarcomTestBuilder().Build();

            //Assert
            garcom.EhValido().IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Sem Nome Deve Falhar")]
        [Trait(nameof(Garcom), "Validar")]
        public void Validar_SemNome_DeveFalhar()
        {
            //Arrange
            var garcom = new GarcomTestBuilder()
                .ComNome("")
                .Build();

            //Act 
            Result result = garcom.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.GarcomNomeEhObrigatorio);
        }

        [Fact(DisplayName = "Nome Acima Limite Caracteres Deve Falhar")]
        [Trait(nameof(Garcom), "Validar")]
        public void Validar_NomeAcimaLimiteCaracteres_DeveFalhar()
        {
            //Arrange
            var garcom = new GarcomTestBuilder()
                .ComNome("José da Silva".PadRight(GarconsConstantes.GarcomTamanhoMaximoNome + 5))
                .Build();

            //Act 
            Result result = garcom.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.NomeDeveTerAteCaracteres);
        }

        [Fact(DisplayName = "Apelido Acima Limite Caracteres Deve Falhar")]
        [Trait(nameof(Garcom), "Validar")]
        public void Validar_ApelidoAcimaLimiteCaracteres_DeveFalhar()
        {
            //Arrange
            var garcom = new GarcomTestBuilder()
                .ComApelido("Garçom".PadRight(GarconsConstantes.GarcomTamanhoMaximoApelido + 5))
                .Build();

            //Act 
            Result result = garcom.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.ApelidoDeveTerAteCaracteres);
        }

        [Fact(DisplayName = "Sem Setores de Atendimento Deve Falhar")]
        [Trait(nameof(Garcom), "Validar")]
        public void Validar_SemSetor_DeveFalhar()
        {
            //Arrange
            var garcom = new GarcomTestBuilder()
                .ComSetores(new List<Setor>())
                .Build();

            //Act 
            Result result = garcom.EhValido();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.GarcomDeveTerNoMinimoUmSetorVinculado);
        }

        [Fact(DisplayName = "Adicionar Setor Deve Ter Sucesso")]
        [Trait(nameof(Garcom), nameof(Garcom.AdicionarSetor))]
        public void Garcom_AdicionarSetor_DeveTerSucesso()
        {
            //Arrange
            var garcom = new GarcomTestBuilder()
                .ComSetores(new List<Setor> { new SetorTestBuilder().ComId(1).Build() })
                .Build();
            int quantidade = garcom.SetoresAtendimento.Count;

            //Act
            Result result = garcom.AdicionarSetor(new SetorTestBuilder().ComId(2).Build());

            //Assert
            result.IsSuccess.ShouldBeTrue();
            garcom.SetoresAtendimento.Count.ShouldBe(quantidade + 1);
        }

        [Fact(DisplayName = "Adicionar Setor Existente Deve Ter Falhar")]
        [Trait(nameof(Garcom), nameof(Garcom.AdicionarSetor))]
        public void AdicionarSetor_SetorJaAdicionado_DeveTerSucesso()
        {
            //Arrange
            var setor = new SetorTestBuilder().ComId(1).Build();
            var garcom = new GarcomTestBuilder()
                .ComSetores(new List<Setor> { setor })
                .Build();

            //Act
            Result result = garcom.AdicionarSetor(setor);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.SetorJaFoiVinculadoAoGarcom);
        }

        [Fact(DisplayName = "Remover Setor Deve Ter Sucesso")]
        [Trait(nameof(Garcom), nameof(Garcom.RemoverSetor))]
        public void Garcom_RemoverSetor_DeveTerSucesso()
        {
            //Arrange
            var setorBuilder = new SetorTestBuilder();
            var garcom = new GarcomTestBuilder()
                .ComSetores(new List<Setor> { setorBuilder.ComId(1).Build(), setorBuilder.ComId(2).Build() })
                .Build();

            //Act
            Result result = garcom.RemoverSetor(1);

            //Assert
            result.IsSuccess.ShouldBeTrue();
        }


        [Fact(DisplayName = "Remover Setor Inexistente Deve Ter Falhar")]
        [Trait(nameof(Garcom), nameof(Garcom.RemoverSetor))]
        public void RemoverSetor_SetorInexistente_DeveTerSucesso()
        {
            //Arrange
            var setorBuilder = new SetorTestBuilder();
            var garcom = new GarcomTestBuilder()
                .ComSetores(new List<Setor> { setorBuilder.ComId(1).Build(), setorBuilder.ComId(2).Build() })
                .Build();

            //Act
            int setorRemover = 3;
            Result result = garcom.RemoverSetor(setorRemover);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(GarconsConstantes.SetorInformadoNaoFoiLocalizado, setorRemover));
        }

    }
}
