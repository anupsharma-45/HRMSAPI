CREATE TABLE [dbo].[UserOrganizations](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [UserId] [uniqueidentifier] NOT NULL,
    [OrganizationId] [uniqueidentifier] NOT NULL,
    CONSTRAINT [FK_UserOrgs_Users] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id]),
    CONSTRAINT [FK_UserOrgs_Orgs] FOREIGN KEY([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id])
);