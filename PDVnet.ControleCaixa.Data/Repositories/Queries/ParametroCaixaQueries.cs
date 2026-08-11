namespace PDVnet.ControleCaixa.Data.Repositories.Queries
{
    public class ParametroCaixaQueries
    {
        public const string ObterSaldoMinimo = @"
        SELECT TOP 1
            SaldoMinimoAlerta
        FROM dbo.ParametroCaixa;";

        public const string AtualizarSaldoMinimo = @"
        UPDATE dbo.ParametroCaixa
        SET SaldoMinimoAlerta = @SaldoMinimoAlerta
        WHERE Id = 1;";
    }
}