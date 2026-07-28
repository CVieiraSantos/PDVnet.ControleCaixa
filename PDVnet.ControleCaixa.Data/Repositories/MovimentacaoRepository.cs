using Microsoft.Data.SqlClient;
using PDVnet.ControleCaixa.Data.Connection;
using PDVnet.ControleCaixa.Data.Repositories.Interfaces;
using PDVnet.ControleCaixa.Data.Repositories.Queries;
using PDVnet.ControleCaixa.Model.Entities;
using PDVnet.ControleCaixa.Model.Enums;
using System.Data;

namespace PDVnet.ControleCaixa.Data.Repositories
{
    public class MovimentacaoRepository : IMovimentacaoRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public MovimentacaoRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task AtualizarAsync(Movimentacao movimentacao)
        {
            await using SqlConnection connection = _connectionFactory.Create();

            await connection.OpenAsync();

            await using SqlCommand command = new(
                MovimentacaoQueries.Atualizar,
                connection);

            command.CommandType = CommandType.Text;

            command.Parameters.Add("@Id", SqlDbType.Int)
                              .Value = movimentacao.Id;

            command.Parameters.Add("@Descricao", SqlDbType.NVarChar, 200)
                              .Value = movimentacao.Descricao;

            command.Parameters.Add("@Tipo", SqlDbType.Int)
                              .Value = (int)movimentacao.Tipo;

            command.Parameters.Add("@Categoria", SqlDbType.NVarChar, 100)
                              .Value = movimentacao.Categoria ?? (object)DBNull.Value;

            var valorParameter = command.Parameters.Add("@Valor", SqlDbType.Decimal);
            valorParameter.Precision = 10;
            valorParameter.Scale = 2;
            valorParameter.Value = movimentacao.Valor;

            command.Parameters.Add("@Status", SqlDbType.Bit)
                              .Value = movimentacao.Status;

            await command.ExecuteNonQueryAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            await using SqlConnection connection = _connectionFactory.Create();

            await connection.OpenAsync();

            await using SqlCommand command = new(
                MovimentacaoQueries.Excluir,
                connection);

            command.CommandType = CommandType.Text;

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await command.ExecuteNonQueryAsync();
        }

        public async Task<int> InserirAsync(Movimentacao movimentacao)
        {   
            if(movimentacao == null)
                throw new ArgumentNullException(nameof(movimentacao));

            await using SqlConnection connection = _connectionFactory.Create();

            await connection.OpenAsync();

            await using SqlCommand command = new SqlCommand(MovimentacaoQueries.Inserir, connection);

            command.CommandType = CommandType.Text;

            command.Parameters.Add("@Descricao", SqlDbType.NVarChar, 200)
                              .Value = movimentacao.Descricao;

            command.Parameters.Add("@Tipo", SqlDbType.Int)
                              .Value = (int)movimentacao.Tipo;

            command.Parameters.Add("@Categoria", SqlDbType.NVarChar, 100)
                             .Value = movimentacao.Categoria is null
                                  ? DBNull.Value
                                  : movimentacao.Categoria;

            var valorParameter = command.Parameters.Add("@Valor", SqlDbType.Decimal);
            valorParameter.Precision = 10;
            valorParameter.Scale = 2;
            valorParameter.Value = movimentacao.Valor;

            command.Parameters.Add("@Status", SqlDbType.Bit)
                              .Value = movimentacao.Status;

            object? result = await command.ExecuteScalarAsync();

            if (result is null || result == DBNull.Value)
            {
                throw new InvalidOperationException(
                    "Não foi possível recuperar o identificador da movimentação cadastrada.");
            }

            return Convert.ToInt32(result);
        }

        public async Task<Movimentacao?> ObterPorIdAsync(int id)
        {
            await using SqlConnection connection = _connectionFactory.Create();

            await connection.OpenAsync();

            await using SqlCommand command = new(
                MovimentacaoQueries.ObterPorId,
                connection);

            command.CommandType = CommandType.Text;

            command.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return MapearMovimentacao(reader);
            }

            return null;
        }

        public async Task<decimal> ObterSaldoAsync()
        {
            await using SqlConnection connection = _connectionFactory.Create();

            await connection.OpenAsync();

            await using SqlCommand command = new(
                MovimentacaoQueries.ObterSaldo,
                connection);

            command.CommandType = CommandType.Text;

            object ? resultado = await command.ExecuteScalarAsync();

            return resultado is null || resultado is DBNull ? 0 : Convert.ToDecimal(resultado);
        }

        public async Task<IReadOnlyList<Movimentacao>> ObterTodasAsync()
        {
            List<Movimentacao> movimentacoes = new List<Movimentacao> ();

            await using SqlConnection connection = _connectionFactory.Create();

            await connection.OpenAsync();

            await using SqlCommand command = new SqlCommand(
                MovimentacaoQueries.ObterTodas,
                connection);

            command.CommandType = CommandType.Text;

            await using SqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                movimentacoes.Add(MapearMovimentacao(reader));
            }

            return movimentacoes;
        }

        private static Movimentacao MapearMovimentacao(SqlDataReader reader)
        {
            return new Movimentacao
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),

                Descricao = reader.GetString(reader.GetOrdinal("Descricao")),

                Tipo = (TipoMovimentacao)reader.GetInt32(reader.GetOrdinal("Tipo")),

                Categoria = reader.IsDBNull(reader.GetOrdinal("Categoria"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("Categoria")),

                Valor = reader.GetDecimal(reader.GetOrdinal("Valor")),

                DataMovimento = reader.GetDateTime(reader.GetOrdinal("DataMovimento")),

                Status = reader.GetBoolean(reader.GetOrdinal("Status"))
            };
        }
    }
}
