/******************************************************************************
    Projeto : PDVnet Controle de Caixa
    Objetivo: Criação do banco de dados e estrutura inicial
******************************************************************************/

IF DB_ID('PDVnetControleCaixa') IS NULL
BEGIN
    CREATE DATABASE PDVnetControleCaixa;
END;
GO

USE PDVnetControleCaixa;
GO

IF OBJECT_ID('dbo.MovimentacaoCaixa', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MovimentacaoCaixa
    (
        Id INT IDENTITY(1,1) NOT NULL,

        Descricao NVARCHAR(200) NOT NULL,

        -- 1 = Entrada | 2 = Saída
        Tipo INT NOT NULL,

        Categoria NVARCHAR(100) NULL,

        Valor DECIMAL(10,2) NOT NULL,

        DataMovimento DATETIME NOT NULL
            CONSTRAINT DF_MovimentacaoCaixa_DataMovimento
            DEFAULT (GETDATE()),

        Status BIT NOT NULL

        CONSTRAINT PK_MovimentacaoCaixa
            PRIMARY KEY CLUSTERED (Id),

        CONSTRAINT CK_MovimentacaoCaixa_Tipo
            CHECK (Tipo IN (1,2)),

        CONSTRAINT CK_MovimentacaoCaixa_Valor
            CHECK (Valor > 0)
    );
END;
GO

IF OBJECT_ID('dbo.ParametroCaixa', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ParametroCaixa
    (
        Id INT NOT NULL,

        -- Valor mínimo de saldo usado para disparar o alerta de "saldo baixo"
        -- no dashboard. Configurável pela própria aplicação (ver PesquisarAsync
        -- / tela principal), não é fixo no código.
        SaldoMinimoAlerta DECIMAL(10,2) NOT NULL,

        CONSTRAINT PK_ParametroCaixa
            PRIMARY KEY CLUSTERED (Id),

        CONSTRAINT CK_ParametroCaixa_SaldoMinimoAlerta
            CHECK (SaldoMinimoAlerta > 0)
    );
END;
GO

-- Linha única (Id = 1) com o valor padrão sugerido no desafio (R$ 100,00).
-- Só insere se ainda não existir, então o script continua idempotente.
IF NOT EXISTS (SELECT 1 FROM dbo.ParametroCaixa WHERE Id = 1)
BEGIN
    INSERT INTO dbo.ParametroCaixa (Id, SaldoMinimoAlerta)
    VALUES (1, 100.00);
END;
GO
