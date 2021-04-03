﻿using HowToDevelop.HealthFood.Setores;
using HowToDevelop.HealthFood.Infraestrutura.Tests.Builders;
using Shouldly;
using Xunit;

namespace HowToDevelop.HealthFood.Infraestrutura.Tests.Dominio.Setores
{
    public class MesaTests
    {
        public MesaTests()
        {

        }

        [Fact(DisplayName = "Mesa Válida Deve Ter Sucesso")]
        [Trait(nameof(Mesa), "Criar")]
        public void Setor_Criar_DeveTerSucesso()
        {
            //Arrange & Act
            var setor = new MesaTestBuilder().Build();

            //Assert
            setor.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Numeracao Igual a Zero Falhar")]
        [Trait(nameof(HealthFood.Setores.Setor), "Validar")]
        public void Validar_NumeracaoIgualAZero_DeveFalhar()
        {
            //Arrange 
            var builder = new MesaTestBuilder()
                .ComNumeracao(0);

            //Act
            var result = builder.Build();

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(SetoresConstantes.NumeracaoNaoPodeSerIgualZero);
        }
    }
}
