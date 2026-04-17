CREATE TABLE [dbo].[UserRoles](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [UserId] [uniqueidentifier] NOT NULL,
    [RoleId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY([RoleId]) REFERENCES [dbo].[Roles] ([Id])
);