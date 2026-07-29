using PDVnet.ControleCaixa.Business.Exceptions;
using PDVnet.ControleCaixa.Business.Interfaces;
using PDVnet.ControleCaixa.Business.Validators;
using PDVnet.ControleCaixa.Data.Repositories.Interfaces;
using PDVnet.ControleCaixa.Model.Entities;
using PDVnet.ControleCaixa.Model.Filters;

namespace PDVnet.ControleCaixa.Business.Services
{
    public sealed class MovimentacaoService : IMovimentacaoService
    {
        private readonly IMovimentacaoRepository _repository;

        public MovimentacaoService(IMovimentacaoRepository movimentacaoRepository)
        {
            _repository = movimentacaoRepository;
        }

        public async Task AtualizarAsync(Movimentacao movimentacao)
        {
            MovimentacaoValidator.Validar(movimentacao);
            
            Movimentacao? movimentacaoExistente = await _repository.ObterPorIdAsync(movimentacao.Id);

            if (movimentacaoExistente is null)
                throw new EntidadeNaoEncontradaException("Movimentação não encontrada.");
                
            await _repository.AtualizarAsync(movimentacao);
        }

        public async Task ExcluirAsync(int id)
        {
            Movimentacao? movimentacao =
            await _repository.ObterPorIdAsync(id);

            if (movimentacao is null)
                throw new EntidadeNaoEncontradaException(
                    "Movimentação não encontrada.");

            await _repository.ExcluirAsync(id);
        }

        public async Task<int> InserirAsync(Movimentacao movimentacao)
        {
            MovimentacaoValidator.Validar(movimentacao);

            movimentacao.Status = true;

            return await _repository.InserirAsync(movimentacao);
        }

        public async Task<Movimentacao?> ObterPorIdAsync(int id)
        {
            Movimentacao? movimentacao =
            await _repository.ObterPorIdAsync(id);

            if (movimentacao is null)
            {
                throw new EntidadeNaoEncontradaException(
                    "Movimentação não encontrada.");
            }

            return movimentacao;
        }

        public async Task<decimal> ObterSaldoAsync()
        {
            return await _repository.ObterSaldoAsync();
        }

        public async Task<IReadOnlyList<Movimentacao>> ObterTodasAsync()
        {
            return await _repository.ObterTodasAsync();
        }

        public async Task<IReadOnlyList<Movimentacao>> PesquisarAsync(MovimentacaoFiltro filtro)
        {
            return await _repository.PesquisarAsync(filtro);
        }
    }
}
