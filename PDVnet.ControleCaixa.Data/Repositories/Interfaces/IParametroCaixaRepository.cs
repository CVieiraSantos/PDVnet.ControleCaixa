namespace PDVnet.ControleCaixa.Data.Repositories.Interfaces {
    public interface IParametroCaixaRepository
    {
        Task<decimal> ObterSaldoMinimoAsync();

        Task AtualizarSaldoMinimoAsync(decimal saldoMinimo);
    }
}
