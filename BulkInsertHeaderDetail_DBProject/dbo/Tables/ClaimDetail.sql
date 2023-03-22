CREATE TABLE [dbo].[ClaimDetail] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL,
    [HeaderId]             INT           NOT NULL,
    [ClaimDate]            DATETIME      NOT NULL,
    [TariffCode]           VARCHAR (20)  NOT NULL,
    [ProcedureDescription] VARCHAR (300) NOT NULL,
    [AmountPaid]           MONEY         NOT NULL,
    [AmountClaimed]        MONEY         NOT NULL,
    [TariffAmount]         MONEY         NOT NULL,
    [PaidToProvider]       MONEY         NOT NULL,
    [PaidToMember]         MONEY         NOT NULL,
    CONSTRAINT [PK_ClaimDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ClaimDetail_ClaimHeader] FOREIGN KEY ([HeaderId]) REFERENCES [dbo].[ClaimHeader] ([Id])
);

