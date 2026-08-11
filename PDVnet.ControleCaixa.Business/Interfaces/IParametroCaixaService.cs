namespace PDVnet.ControleCaixa.Business.Interfaces
{
    public interface IParametroCaixaService
    {
        Task<decimal> ObterSaldoMinimoAsync();

        Task AtualizarSaldoMinimoAsync(decimal saldoMinimo);
    }
}
