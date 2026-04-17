CREATE TABLE [dbo].[Permissions](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [Name] [nvarchar](100) NOT NULL,
    [Code] [nvarchar](50) NOT NULL,
    [Module] [nvarchar](50) NOT NULL,
    [CreatedAt] [datetime2](7) NOT NULL,
    [UpdatedAt] [datetime2](7) NOT NULL
);