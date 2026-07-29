using PDVnet.ControleCaixa.Business.Exceptions;
using PDVnet.ControleCaixa.Business.Validators;
using PDVnet.ControleCaixa.Model.Entities;
using PDVnet.ControleCaixa.Model.Enums;

namespace PDVnet.ControleCaixa.Tests.Business.Validators
{
    public class MovimentacaoValidatorTests
    {
        [Fact]
        public void Validar_DeveLancarArgumentNullException_QuandoMovimentacaoForNula()
        {
            Assert.Throws<ArgumentNullException>(() => MovimentacaoValidator.Validar(null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Validar_DeveLancarValidacaoException_QuandoDescricaoForInvalida(string? descricao)
        {
            Movimentacao movimentacao = new()
            {
                Descricao = descricao!,
                Valor = 100,
                Tipo = TipoMovimentacao.Entrada
            };

            ValidacaoException ex = Assert.Throws<ValidacaoException>(
                () => MovimentacaoValidator.Validar(movimentacao));

            Assert.Equal("A descrição é obrigatória.", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public void Validar_DeveLancarValidacaoException_QuandoValorForMenorOuIgualAZero(decimal valor)
        {
            Movimentacao movimentacao = new()
            {
                Descricao = "Venda",
                Valor = valor,
                Tipo = TipoMovimentacao.Entrada
            };

            ValidacaoException ex = Assert.Throws<ValidacaoException>(
                () => MovimentacaoValidator.Validar(movimentacao));

            Assert.Equal("O valor deve ser maior que zero.", ex.Message);
        }

        [Fact]
        public void Validar_DeveLancarValidacaoException_QuandoTipoForInvalido()
        {
            Movimentacao movimentacao = new()
            {
                Descricao = "Venda",
                Valor = 100,
                Tipo = (TipoMovimentacao)999
            };

            Assert.Throws<ValidacaoException>(() => MovimentacaoValidator.Validar(movimentacao));
        }

        [Fact]
        public void Validar_NaoDeveLancarExcecao_QuandoMovimentacaoForValida()
        {
            Movimentacao movimentacao = new()
            {
                Descricao = "Venda balcão",
                Valor = 150.50m,
                Tipo = TipoMovimentacao.Entrada
            };

            Exception? exception = Record.Exception(() => MovimentacaoValidator.Validar(movimentacao));

            Assert.Null(exception);
        }
    }
}