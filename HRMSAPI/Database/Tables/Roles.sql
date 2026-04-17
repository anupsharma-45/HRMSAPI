CREATE TABLE [dbo].[Roles](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](250) NULL,
    [CreatedAt] [datetime2](7) NOT NULL,
    [UpdatedAt] [datetime2](7) NOT NULL
);