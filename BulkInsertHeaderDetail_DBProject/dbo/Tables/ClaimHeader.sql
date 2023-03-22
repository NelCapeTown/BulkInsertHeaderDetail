CREATE TABLE [dbo].[ClaimHeader] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [TreatmentDate] DATETIME      NOT NULL,
    [Beneficiary]   VARCHAR (200) NOT NULL,
    [Provider]      VARCHAR (200) NOT NULL,
    [AmountPaid]    MONEY         NOT NULL,
    [AmountClaimed] MONEY         NOT NULL,
    CONSTRAINT [PK_ClaimHeader] PRIMARY KEY CLUSTERED ([Id] ASC)
);

