CREATE TABLE [dbo].[RefreshTokens](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [UserId] [uniqueidentifier] NOT NULL,
    [Token] [nvarchar](max) NOT NULL,
    [ExpiryDate] [datetime2](7) NOT NULL,
    [IsRevoked] [bit] NOT NULL DEFAULT 0,
    [CreatedAt] [datetime2](7) NOT NULL,
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY([UserId]) REFERENCES [dbo].[Users] ([Id])
);