using HowToDevelop.Core.ValidacoesPadrao;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace HowToDevelop.Core.Tests
{
    public class ValidacoesTests
    {
        [Theory(DisplayName = "String Null ou Vazia Deve Retornar Falha")]
        [Trait(nameof(Validacoes), "String Null ou Vazia")]
        [InlineData("")]
        [InlineData(null)]
        public void NaoDeveSerNuloOuVazio_StringNullOuVazia_DeveRetornarFalha(string valor)
        {
            //Arrange
            string mensagem = "String não pode ser nula";

            //Act
            var result = valor.NaoDeveSerNuloOuVazio(mensagem);

            //Asserts
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(mensagem);
        }

        [Fact(DisplayName = "String Preenchida Deve Retornar Sucesso")]
        [Trait(nameof(Validacoes), "String Preenchida")]
        public void NaoDeveSerNuloOuVazio_StringPreenchida_DeveRetornarSucesso()
        {
            //Arrange
            string valor = "String Preenchida";
            string mensagem = "String não pode ser nula";

            //Act
            var result = valor.NaoDeveSerNuloOuVazio(mensagem);

            //Asserts
            result.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Objeto Genérico Null Deve Retornar Falha")]
        [Trait(nameof(Validacoes), "Objeto Genérico Null")]
        public void NaoDeveSerNulo_ObjetoNulo_DeveRetornarFalha()
        {
            //Arrange
            object o = null;
            string mensagem = "Objeto Não Deve Ser Nulo";


            //Act
            var result = o.NaoDeveSerNulo(mensagem);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(mensagem);
        }

        [Fact(DisplayName = "Objeto Genérico Válido Deve Ter Sucesso")]
        [Trait(nameof(Validacoes), "Objeto Genérico Válido")]
        public void NaoDeveSerNulo_ObjetoValido_DeveTerSucesso()
        {
            //Arrange
            object o = new { Id = 1, Nome = "João da Silva" };
            string mensagem = "Objeto Não Deve Ser Nulo";


            //Act
            var result = o.NaoDeveSerNulo(mensagem);

            //Assert
            result.IsSuccess.ShouldBeTrue();
        }

        private List<string> ObterLista(int quantidadeItens)
        {
            List<string> lista = new List<string>();
            for (int i = 1; i <= quantidadeItens; i++)
                lista.Add($"string {i}");
            return lista;
        }

        [Theory(DisplayName = "Lista Quantidade Itens Inferior ao Mínimo Deve Retornar Falha")]
        [Trait(nameof(Validacoes), "Lista Quantidade Itens Inferior ao Mínimo")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(10)]
        [InlineData(23)]
        public void DeveTerNoMinimo_ListaQuantidadeItensInferiorAoMinimo_DeveFalhar(int quantidadeItens)
        {
            //Arrange
            List<string> lista = ObterLista(quantidadeItens-1);
            string mensagem = $"Quantidade de Itens é inferior a {quantidadeItens}";

            //Act
            var result = lista.DeveTerNoMinimo(quantidadeItens, mensagem);

            //Assert
            result.IsFailure.ShouldBeTrue();
            result.Error.ShouldContain(mensagem);
        }

        [Theory(DisplayName = "Lista Quantidade Itens Igual ou maior que Mínimo Deve Retornar Sucesso")]
        [Trait(nameof(Validacoes), "Lista Quantidade Itens Igual ou maior que Mínimo")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(23)]
        public void DeveTerNoMinimo_ListaQuantidadeIgualOuMaiorMinimo_DeveTerSucesso(int quantidadeItens)
        {
            //Arrange
            
            //Se for par testo colocando itens superior ao limite
            int quantidade = (quantidadeItens % 2 == 0) ? quantidadeItens + 3 : quantidadeItens;
            
            List<string> lista = ObterLista(quantidade);
            string mensagem = $"Quantidade de Itens é inferior a {quantidadeItens}";

            //Act
            var result = lista.DeveTerNoMinimo(quantidadeItens, mensagem);

            //Assert
            result.IsSuccess.ShouldBeTrue();
        }
    }
}
