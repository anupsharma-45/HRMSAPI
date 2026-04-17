CREATE TABLE [dbo].[RolePermissions](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [RoleId] [uniqueidentifier] NOT NULL,
    [PermissionId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [FK_RolePermissions_Roles] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles] ([Id]),
    CONSTRAINT [FK_RolePermissions_Permissions] FOREIGN KEY([PermissionId]) REFERENCES [dbo].[Permissions] ([Id])
);