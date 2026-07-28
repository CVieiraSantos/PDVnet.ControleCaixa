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