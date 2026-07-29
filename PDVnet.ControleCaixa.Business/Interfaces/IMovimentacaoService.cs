using PDVnet.ControleCaixa.Model.Entities;
using PDVnet.ControleCaixa.Model.Filters;

namespace PDVnet.ControleCaixa.Business.Interfaces
{
    public interface IMovimentacaoService
    {
        Task<int> InserirAsync(Movimentacao movimentacao);

        Task AtualizarAsync(Movimentacao movimentacao);

        Task ExcluirAsync(int id);

        Task<IReadOnlyList<Movimentacao>> ObterTodasAsync();

        Task<Movimentacao?> ObterPorIdAsync(int id);

        Task<decimal> ObterSaldoAsync();
        
        Task<IReadOnlyList<Movimentacao>> PesquisarAsync(MovimentacaoFiltro filtro);
    }
}
