using Moq;
using PDVnet.ControleCaixa.Business.Exceptions;
using PDVnet.ControleCaixa.Business.Services;
using PDVnet.ControleCaixa.Data.Repositories.Interfaces;
using PDVnet.ControleCaixa.Model.Entities;
using PDVnet.ControleCaixa.Model.Enums;

namespace PDVnet.ControleCaixa.Tests.Business.Services
{
    public class MovimentacaoServiceTests
    {
        private readonly Mock<IMovimentacaoRepository> _repositoryMock;
        private readonly MovimentacaoService _service;

        public MovimentacaoServiceTests()
        {
            _repositoryMock = new Mock<IMovimentacaoRepository>();
            _service = new MovimentacaoService(_repositoryMock.Object);
        }

        [Fact]
        public async Task InserirAsync_DeveDefinirStatusComoTrue_EChamarRepositorio()
        {
            Movimentacao movimentacao = new()
            {
                Descricao = "Venda",
                Valor = 100,
                Tipo = TipoMovimentacao.Entrada,
                Status = false
            };

            _repositoryMock
                .Setup(r => r.InserirAsync(It.IsAny<Movimentacao>()))
                .ReturnsAsync(1);

            int id = await _service.InserirAsync(movimentacao);

            Assert.Equal(1, id);
            Assert.True(movimentacao.Status);
            _repositoryMock.Verify(r => r.InserirAsync(movimentacao), Times.Once);
        }

        [Fact]
        public async Task InserirAsync_DeveLancarValidacaoException_ENaoDeveChamarRepositorio_QuandoInvalida()
        {
            Movimentacao movimentacao = new() { Descricao = "", Valor = 100 };

            await Assert.ThrowsAsync<ValidacaoException>(() => _service.InserirAsync(movimentacao));

            _repositoryMock.Verify(r => r.InserirAsync(It.IsAny<Movimentacao>()), Times.Never);
        }

        [Fact]
        public async Task AtualizarAsync_DeveLancarEntidadeNaoEncontradaException_QuandoNaoExistir()
        {
            Movimentacao movimentacao = new()
            {
                Id = 99,
                Descricao = "Venda",
                Valor = 100,
                Tipo = TipoMovimentacao.Entrada
            };

            _repositoryMock
                .Setup(r => r.ObterPorIdAsync(99))
                .ReturnsAsync((Movimentacao?)null);

            await Assert.ThrowsAsync<EntidadeNaoEncontradaException>(
                () => _service.AtualizarAsync(movimentacao));

            _repositoryMock.Verify(r => r.AtualizarAsync(It.IsAny<Movimentacao>()), Times.Never);
        }

        [Fact]
        public async Task ExcluirAsync_DeveLancarEntidadeNaoEncontradaException_QuandoIdNaoExistir()
        {
            _repositoryMock
                .Setup(r => r.ObterPorIdAsync(1))
                .ReturnsAsync((Movimentacao?)null);

            await Assert.ThrowsAsync<EntidadeNaoEncontradaException>(() => _service.ExcluirAsync(1));
        }

        [Fact]
        public async Task ExcluirAsync_DeveChamarRepositorio_QuandoIdExistir()
        {
            Movimentacao movimentacao = new() { Id = 1 };

            _repositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(movimentacao);

            await _service.ExcluirAsync(1);

            _repositoryMock.Verify(r => r.ExcluirAsync(1), Times.Once);
        }

        [Fact]
        public async Task ObterSaldoAsync_DeveRetornarValorDoRepositorio()
        {
            _repositoryMock.Setup(r => r.ObterSaldoAsync()).ReturnsAsync(1500.75m);

            decimal saldo = await _service.ObterSaldoAsync();

            Assert.Equal(1500.75m, saldo);
        }
    }
}