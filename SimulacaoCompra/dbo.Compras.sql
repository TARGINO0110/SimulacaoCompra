CREATE TABLE [dbo].[Compras] (
    [Idcompra]     INT             IDENTITY (1, 1) NOT NULL,
    [Datacompra]   DATETIME2 (7)   NOT NULL,
    [Qtdparcelas]  DECIMAL (2)     NOT NULL,
    [Valorjuros]   DECIMAL (10, 4) NOT NULL,
    [Valorparcela] DECIMAL (10, 2) NULL,
    [Valortotal]   DECIMAL (10, 2) NOT NULL,
    CONSTRAINT [PK_Compras] PRIMARY KEY CLUSTERED ([Idcompra] ASC)
);

