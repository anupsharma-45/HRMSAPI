CREATE TABLE [dbo].[AuditLogs](
    [Id] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [UserId] [uniqueidentifier] NULL,
    [Action] [nvarchar](100) NOT NULL,
    [Module] [nvarchar](250) NOT NULL,
    [Timestamp] [datetime2](7) NOT NULL,
    [IPAddress] [nvarchar](50) NULL
);