CREATE TABLE [dbo].[Users](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [FirstName] [nvarchar](100) NOT NULL,
    [LastName] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](100) NOT NULL UNIQUE,
    [PhoneNumber] [nvarchar](20) NULL, -- Mapped from 'Phone' in code
    [Password] [nvarchar](max) NOT NULL, -- Mapped from 'PasswordHash' in code
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [IsDeleted] [bit] NOT NULL DEFAULT 0,
    [LastLoginAt] [datetime2](7) NULL,
    [CreatedAt] [datetime2](7) NOT NULL,
    [UpdatedAt] [datetime2](7) NOT NULL
);