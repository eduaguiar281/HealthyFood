using CSharpFunctionalExtensions;
using HowToDevelop.Core.ObjetosDeValor;
using HowToDevelop.HealthFood.Garcons;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;
using Shouldly;
using Xunit;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Dominio.Garcons
{
    public class GarcomTests
    {
        [Fact(DisplayName = "Criar Válido Deve Ter Sucesso")]
        [Trait(nameof(Garcom), nameof(Garcom.Criar))]
        public void Garcom_Validar_DeveTerSucesso()
        {
            // Arrange & Act
            var garcom = new GarcomTestBuilder().Build();

            // Assert
            garcom.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Vincular Setor Deve Ter Sucesso")]
        [Trait(nameof(Garcom), nameof(Garcom.VincularSetor))]
        public void Garcom_AdicionarSetor_DeveTerSucesso()
        {
            // Arrange
            var garcom = new GarcomTestBuilder()
                .Build().Value;
            int quantidade = garcom.SetoresAtendimento.Count;

            // Act
            Result result = garcom.VincularSetor(1);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            garcom.SetoresAtendimento.Count.ShouldBe(quantidade + 1);
        }

        [Fact(DisplayName = "Vincular Setor Existente Deve Falhar")]
        [Trait(nameof(Garcom), nameof(Garcom.VincularSetor))]
        public void AdicionarSetor_SetorJaAdicionado_DeveFalhar()
        {
            // Arrange
            var garcom = new GarcomTestBuilder().Build().Value;
            garcom.VincularSetor(1);

            // Act
            Result result = garcom.VincularSetor(1);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.SetorJaFoiVinculadoAoGarcom);
        }

        [Fact(DisplayName = "Vincular Setor Inválido Deve Falhar")]
        [Trait(nameof(Garcom), nameof(Garcom.VincularSetor))]
        public void AdicionarSetor_SetorInvalido_DeveFalhar()
        {
            // Arrange
            var garcom = new GarcomTestBuilder().Build().Value;

            // Act
            Result result = garcom.VincularSetor(0);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(GarconsConstantes.SetorIdNaoEhValido);
        }

        [Fact(DisplayName = "Remover Setor Deve Ter Sucesso")]
        [Trait(nameof(Garcom), nameof(Garcom.RemoverVinculoDeSetor))]
        public void Garcom_RemoverSetor_DeveTerSucesso()
        {
            // Arrange
            var garcom = new GarcomTestBuilder()
                .Build().Value;
            garcom.VincularSetor(1);
            garcom.VincularSetor(2);

            // Act
            Result result = garcom.RemoverVinculoDeSetor(2);

            // Assert
            result.IsSuccess.ShouldBeTrue();
        }


        [Fact(DisplayName = "Remover Setor Inexistente Deve Ter Falhar")]
        [Trait(nameof(Garcom), nameof(Garcom.RemoverVinculoDeSetor))]
        public void RemoverSetor_SetorInexistente_DeveTerSucesso()
        {
            // Arrange
            var garcom = new GarcomTestBuilder()
                .Build().Value;
            garcom.VincularSetor(1);
            garcom.VincularSetor(2);
            int setorRemover = 3;

            // Act
            Result result = garcom.RemoverVinculoDeSetor(setorRemover);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(string.Format(GarconsConstantes.SetorInformadoNaoFoiLocalizado, setorRemover));
        }


        [Fact(DisplayName = "Alterar Dados Pessoais Válidos Deve Ter Sucesso")]
        [Trait(nameof(Garcom), nameof(Garcom.AlterarDadosPessoais))]
        public void Garcom_AlterarDadosPessoais_DeveTerSucesso()
        {
            // Arrange
            var garcom = new GarcomTestBuilder()
                .ComNome(Nome.Criar("José da Silva").Value)
                .ComApelido(Apelido.Criar("Zé").Value)
                .Build().Value;
            string novoNome = "José de Souza";
            string novoApelido = "Zezão";

            // Act 
            Result result = garcom.AlterarDadosPessoais(novoNome, novoApelido);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            garcom.Nome.Valor.ShouldBe(novoNome);
            garcom.Apelido.Valor.ShouldBe(novoApelido);
        }

        [Fact(DisplayName = "Alterar Dados Pessoais Dados Inválidos Deve Falhar")]
        [Trait(nameof(Garcom), nameof(Garcom.AlterarDadosPessoais))]
        public void AlterarDadosPessoais_DadosInvalidos_DeveFalhar()
        {
            // Arrange
            var garcom = new GarcomTestBuilder()
                .ComNome(Nome.Criar("José da Silva").Value)
                .ComApelido(Apelido.Criar("Zé").Value)
                .Build().Value;
            string novoNome = "";
            string novoApelido = "Zezão";

            // Act 
            Result result = garcom.AlterarDadosPessoais(novoNome, novoApelido);

            // Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(NomeConstantes.NomeEhObrigatorio);
            garcom.Nome.Valor.ShouldNotBe(novoNome);
            garcom.Apelido.Valor.ShouldNotBe(novoApelido);
        }
    }
}
