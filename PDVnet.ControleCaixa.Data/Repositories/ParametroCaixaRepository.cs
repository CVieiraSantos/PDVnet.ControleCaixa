using Microsoft.Data.SqlClient;
using PDVnet.ControleCaixa.Data.Connection;
using PDVnet.ControleCaixa.Data.Repositories.Interfaces;
using PDVnet.ControleCaixa.Data.Repositories.Queries;
using System.Data;

namespace PDVnet.ControleCaixa.Data.Repositories
{
    public class ParametroCaixaRepository : IParametroCaixaRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ParametroCaixaRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<decimal> ObterSaldoMinimoAsync()
        {
            await using SqlConnection connection = _connectionFactory.Create();

            await connection.OpenAsync();

            await using SqlCommand command = new(
                ParametroCaixaQueries.ObterSaldoMinimo,
                connection);

            command.CommandType = CommandType.Text;

            object? resultado = await command.ExecuteScalarAsync();

            // Se a tabela ainda não tiver sido semeada (script antigo, por
            // exemplo), cai no mesmo valor de exemplo citado no desafio.
            return resultado is null || resultado is DBNull
                ? 100m
                : Convert.ToDecimal(resultado);
        }

        public async Task AtualizarSaldoMinimoAsync(decimal saldoMinimo)
        {
            await using SqlConnection connection = _connectionFactory.Create();

            await connection.OpenAsync();

            await using SqlCommand command = new(
                ParametroCaixaQueries.AtualizarSaldoMinimo,
                connection);

            command.CommandType = CommandType.Text;

            var saldoMinimoParameter = command.Parameters.Add("@SaldoMinimoAlerta", SqlDbType.Decimal);
            saldoMinimoParameter.Precision = 10;
            saldoMinimoParameter.Scale = 2;
            saldoMinimoParameter.Value = saldoMinimo;

            await command.ExecuteNonQueryAsync();
        }
    }
}
