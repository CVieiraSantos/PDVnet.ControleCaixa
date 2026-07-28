namespace PDVnet.ControleCaixa.Data.Repositories.Queries
{
    public static class MovimentacaoQueries
    {
        public const string Inserir = @"
        INSERT INTO MovimentacaoCaixa
        (
            Descricao,
            Tipo,
            Categoria,
            Valor,
            DataMovimento,
            Status
        )
        OUTPUT INSERTED.Id
        VALUES
        (
            @Descricao,
            @Tipo,
            @Categoria,
            @Valor,
            GETDATE(),
            @Status
        );";

        public const string ObterTodas = @"
        SELECT
            Id,
            Descricao,
            Tipo,
            Categoria,
            Valor,
            DataMovimento,
            Status
        FROM dbo.MovimentacaoCaixa;";

        public const string ObterPorId = @"
        SELECT
            Id,
            Descricao,
            Tipo,
            Categoria,
            Valor,
            DataMovimento,
            Status
        FROM dbo.MovimentacaoCaixa
        WHERE Id = @Id;";

        public const string Atualizar = @"
        UPDATE dbo.MovimentacaoCaixa
        SET
            Descricao = @Descricao,
            Tipo = @Tipo,
            Categoria = @Categoria,
            Valor = @Valor,
            Status = @Status
        WHERE Id = @Id;";

        public const string Excluir = @"
        DELETE
        FROM dbo.MovimentacaoCaixa
        WHERE Id = @Id;";

        public const string ObterSaldo = @"
        SELECT
            ISNULL(SUM(
                CASE
                    WHEN Tipo = 1 THEN Valor
                    WHEN Tipo = 2 THEN -Valor
                END
            ), 0)
        FROM dbo.MovimentacaoCaixa;";
    }
}
