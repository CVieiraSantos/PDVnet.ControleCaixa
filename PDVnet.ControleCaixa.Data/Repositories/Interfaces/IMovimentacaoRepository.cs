using PDVnet.ControleCaixa.Model.Entities;

namespace PDVnet.ControleCaixa.Data.Repositories.Interfaces
{
    public interface IMovimentacaoRepository
    {
        Task<int> InserirAsync(Movimentacao movimentacao);

        Task<IReadOnlyList<Movimentacao>> ObterTodasAsync();

        Task<Movimentacao?> ObterPorIdAsync(int id);

        Task AtualizarAsync(Movimentacao movimentacao);

        Task ExcluirAsync(int id);

        Task<decimal> ObterSaldoAsync();
    }
}
