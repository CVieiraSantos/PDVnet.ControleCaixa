using Moq;
using PDVnet.ControleCaixa.Business.Exceptions;
using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Data.Repositories.Interfaces;

namespace PDVnet.ControleCaixa.Tests.Business.Services
{
    public class ParametroCaixaServiceTests
    {
        private readonly Mock<IParametroCaixaRepository> _repositoryMock;
        private readonly ParametroCaixaService _service;

        public ParametroCaixaServiceTests()
        {
            _repositoryMock = new Mock<IParametroCaixaRepository>();
            _service = new ParametroCaixaService(_repositoryMock.Object);
        }

        [Fact]
        public async Task ObterSaldoMinimoAsync_DeveRetornarValorDoRepositorio()
        {
            _repositoryMock
                .Setup(r => r.ObterSaldoMinimoAsync())
                .ReturnsAsync(250m);

            decimal resultado = await _service.ObterSaldoMinimoAsync();

            Assert.Equal(250m, resultado);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public async Task AtualizarSaldoMinimoAsync_DeveLancarValidacaoException_QuandoValorForMenorOuIgualAZero(decimal valor)
        {
            ValidacaoException ex = await Assert.ThrowsAsync<ValidacaoException>(
                () => _service.AtualizarSaldoMinimoAsync(valor));

            Assert.Equal("O saldo mínimo deve ser maior que zero.", ex.Message);

            _repositoryMock.Verify(
                r => r.AtualizarSaldoMinimoAsync(It.IsAny<decimal>()),
                Times.Never);
        }

        [Fact]
        public async Task AtualizarSaldoMinimoAsync_DeveChamarRepositorio_QuandoValorForValido()
        {
            await _service.AtualizarSaldoMinimoAsync(150m);

            _repositoryMock.Verify(r => r.AtualizarSaldoMinimoAsync(150m), Times.Once);
        }
    }
}