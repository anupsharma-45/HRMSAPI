-- 1. Roles Table
CREATE TABLE [dbo].[Roles](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [Name] [nvarchar](100) NOT NULL,
    [Description] [nvarchar](250) NULL,
    [CreatedAt] [datetime2](7) NOT NULL,
    [UpdatedAt] [datetime2](7) NOT NULL
);

-- 2. Users Table
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

-- 3. UserRoles Table
CREATE TABLE [dbo].[UserRoles](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [UserId] [uniqueidentifier] NOT NULL,
    [RoleId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles] ([Id])
);

-- 4. Organizations Table
CREATE TABLE [dbo].[Organizations](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [CompanyName] [nvarchar](200) NOT NULL, -- Mapped from 'Name' in code
    [Code] [nvarchar](50) NOT NULL,
    [Email] [nvarchar](100) NULL,
    [Phone] [nvarchar](20) NULL,
    [Address] [nvarchar](max) NULL,
    [IsActive] [bit] NOT NULL DEFAULT 1,
    [CreatedAt] [datetime2](7) NOT NULL,
    [UpdatedAt] [datetime2](7) NOT NULL
);

-- 5. UserOrganizations Table
CREATE TABLE [dbo].[UserOrganizations](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [UserId] [uniqueidentifier] NOT NULL,
    [OrganizationId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [FK_UserOrgs_Users] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_UserOrgs_Orgs] FOREIGN KEY([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id])
);

-- 6. Permissions Table
CREATE TABLE [dbo].[Permissions](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [Name] [nvarchar](100) NOT NULL,
    [Code] [nvarchar](50) NOT NULL,
    [Module] [nvarchar](50) NOT NULL,
    [CreatedAt] [datetime2](7) NOT NULL,
    [UpdatedAt] [datetime2](7) NOT NULL
);

-- 7. RolePermissions Table
CREATE TABLE [dbo].[RolePermissions](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [RoleId] [uniqueidentifier] NOT NULL,
    [PermissionId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles] ([Id]),
    CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY([PermissionId]) REFERENCES [dbo].[Permissions] ([Id])
);

-- 8. RefreshTokens Table
CREATE TABLE [dbo].[RefreshTokens](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [UserId] [uniqueidentifier] NOT NULL,
    [Token] [nvarchar](max) NOT NULL,
    [ExpiryDate] [datetime2](7) NOT NULL,
    [IsRevoked] [bit] NOT NULL DEFAULT 0,
    [CreatedAt] [datetime2](7) NOT NULL,
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id])
);

-- 9. AuditLogs Table
CREATE TABLE [dbo].[AuditLogs](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [UserId] [uniqueidentifier] NULL,
    [Action] [nvarchar](100) NOT NULL,
    [Module] [nvarchar](250) NOT NULL,
    [Timestamp] [datetime2](7) NOT NULL,
    [IPAddress] [nvarchar](50) NULL
);