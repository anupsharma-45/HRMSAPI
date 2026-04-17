CREATE TABLE [Organizations](
	[Id] [uniqueidentifier] Primary Key NOT NULL,
	[CompanyName] [varchar](200) NOT NULL,
	[GSTIN] [varchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedBy] [uniqueidentifier] NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
	[Address1] [nvarchar](50) NULL,
	[Address2] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[StateCode] [nvarchar](10) NULL,
	[ZipCode] [varchar](25) NULL,
	[CountryCode] [varchar](25) NULL,
	[TimeZone] [varchar](25) NULL
)


