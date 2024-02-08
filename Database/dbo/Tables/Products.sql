CREATE TABLE [dbo].[Products] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NULL,
    [Stock]       INT            NOT NULL,
    [Status]      TINYINT        NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Price]       FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
);

