namespace Authority.DataAccess.Scripts
{
    public static class InitializeScripts
    {
        public const string CheckIfDbExists = @"SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'Authority' and TABLE_NAME = 'Users'";

        public const string CreateTables = @"
            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [Authority].[Claims](
	            [Id] [uniqueidentifier] NOT NULL,
	            [FriendlyName] [nvarchar](128) NOT NULL,
	            [Issuer] [nvarchar](256) NOT NULL,
	            [Type] [nvarchar](256) NOT NULL,
	            [Value] [nvarchar](512) NOT NULL,
	            [Domain_Id] [uniqueidentifier] NULL,
             CONSTRAINT [PK_Authority.Claims] PRIMARY KEY CLUSTERED 
            (
	            [Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [Authority].[Domains](
	            [Id] [uniqueidentifier] NOT NULL,
	            [Name] [nvarchar](128) NOT NULL,
	            [IsActive] [bit] NOT NULL,
             CONSTRAINT [PK_Authority.Domains] PRIMARY KEY CLUSTERED 
            (
	            [Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE TABLE [Authority].[Groups](
	            [Id] [uniqueidentifier] NOT NULL,
	            [Name] [nvarchar](256) NOT NULL,
	            [IsActive] [bit] NOT NULL,
	            [Default] [bit] NOT NULL,
	            [DomainId] [uniqueidentifier] NOT NULL,
             CONSTRAINT [PK_Authority.Groups] PRIMARY KEY CLUSTERED 
            (
	            [Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE TABLE [Authority].[Invites](
	            [Id] [uniqueidentifier] NOT NULL,
	            [Email] [nvarchar](256) NOT NULL,
	            [DomainId] [uniqueidentifier] NOT NULL,
	            [Created] [datetimeoffset](7) NOT NULL,
	            [Accepted] [bit] NOT NULL,
	            [Expire] [datetimeoffset](7) NULL,
             CONSTRAINT [PK_Authority.Invites] PRIMARY KEY CLUSTERED 
            (
	            [Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE TABLE [Authority].[Policies](
	            [Id] [uniqueidentifier] NOT NULL,
	            [DomainId] [uniqueidentifier] NOT NULL,
	            [Name] [nvarchar](max) NOT NULL,
	            [Default] [bit] NOT NULL,
             CONSTRAINT [PK_Authority.Policies] PRIMARY KEY CLUSTERED 
            (
	            [Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE TABLE [Authority].[Users](
	            [DomainId] [uniqueidentifier] NOT NULL,
	            [Email] [nvarchar](128) NOT NULL,
	            [Username] [nvarchar](64) NOT NULL,
	            [PasswordHash] [nvarchar](128) NOT NULL,
	            [Salt] [nvarchar](128) NOT NULL,
	            [IsPending] [bit] NOT NULL,
	            [PendingRegistrationId] [uniqueidentifier] NOT NULL,
	            [IsExternal] [bit] NOT NULL,
	            [ExternalProviderName] [nvarchar](max) NULL,
	            [ExternalToken] [nvarchar](max) NULL,
	            [IsActive] [bit] NOT NULL,
	            [Id] [uniqueidentifier] NOT NULL,
	            [LastLogin] [datetimeoffset](7) NOT NULL,
             CONSTRAINT [PK_Authority.Users] PRIMARY KEY CLUSTERED 
            (
	            [DomainId] ASC,
	            [Email] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE TABLE [Authority].[GroupPolicies](
	            [Group_Id] [uniqueidentifier] NOT NULL,
	            [Policy_Id] [uniqueidentifier] NOT NULL,
             CONSTRAINT [PK_dbo.GroupPolicies] PRIMARY KEY CLUSTERED 
            (
	            [Group_Id] ASC,
	            [Policy_Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE TABLE [Authority].[PolicyAuthorityClaims](
	            [Policy_Id] [uniqueidentifier] NOT NULL,
	            [AuthorityClaim_Id] [uniqueidentifier] NOT NULL,
             CONSTRAINT [PK_dbo.PolicyAuthorityClaims] PRIMARY KEY CLUSTERED 
            (
	            [Policy_Id] ASC,
	            [AuthorityClaim_Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE TABLE [Authority].[UserGroups](
	            [User_DomainId] [uniqueidentifier] NOT NULL,
	            [User_Email] [nvarchar](128) NOT NULL,
	            [Group_Id] [uniqueidentifier] NOT NULL,
             CONSTRAINT [PK_dbo.UserGroups] PRIMARY KEY CLUSTERED 
            (
	            [User_DomainId] ASC,
	            [User_Email] ASC,
	            [Group_Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            GO

            SET ANSI_NULLS ON
            GO
            SET QUOTED_IDENTIFIER ON
            GO
            CREATE TABLE [Authority].[UserPolicies](
	            [User_DomainId] [uniqueidentifier] NOT NULL,
	            [User_Email] [nvarchar](128) NOT NULL,
	            [Policy_Id] [uniqueidentifier] NOT NULL,
             CONSTRAINT [PK_dbo.UserPolicies] PRIMARY KEY CLUSTERED 
            (
	            [User_DomainId] ASC,
	            [User_Email] ASC,
	            [Policy_Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]

            GO
            ALTER TABLE [Authority].[Groups] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [DomainId]
            GO
            ALTER TABLE [Authority].[Invites] ADD  DEFAULT ('0001-01-01T00:00:00.000+00:00') FOR [Created]
            GO
            ALTER TABLE [Authority].[Invites] ADD  DEFAULT ((0)) FOR [Accepted]
            GO
            ALTER TABLE [Authority].[Users] ADD  DEFAULT ('0001-01-01T00:00:00.000+00:00') FOR [LastLogin]
            GO
            ALTER TABLE [Authority].[Claims]  WITH CHECK ADD  CONSTRAINT [FK_Authority.Claims_Authority.Domains_Domain_Id] FOREIGN KEY([Domain_Id])
            REFERENCES [Authority].[Domains] ([Id])
            ON DELETE CASCADE
            GO
            ALTER TABLE [Authority].[Claims] CHECK CONSTRAINT [FK_Authority.Claims_Authority.Domains_Domain_Id]
            GO
            ALTER TABLE [Authority].[Groups]  WITH CHECK ADD  CONSTRAINT [FK_Authority.Groups_Authority.Domains_DomainId] FOREIGN KEY([DomainId])
            REFERENCES [Authority].[Domains] ([Id])
            GO
            ALTER TABLE [Authority].[Groups] CHECK CONSTRAINT [FK_Authority.Groups_Authority.Domains_DomainId]
            GO
            ALTER TABLE [Authority].[Policies]  WITH CHECK ADD  CONSTRAINT [FK_Authority.Policies_Authority.Domains_DomainId] FOREIGN KEY([DomainId])
            REFERENCES [Authority].[Domains] ([Id])
            ON DELETE CASCADE
            GO
            ALTER TABLE [Authority].[Policies] CHECK CONSTRAINT [FK_Authority.Policies_Authority.Domains_DomainId]
            GO
            ALTER TABLE [Authority].[GroupPolicies]  WITH CHECK ADD  CONSTRAINT [FK_Authority.GroupPolicies_Authority.Groups_Group_Id] FOREIGN KEY([Group_Id])
            REFERENCES [Authority].[Groups] ([Id])
            ON DELETE CASCADE
            GO
            ALTER TABLE [Authority].[GroupPolicies] CHECK CONSTRAINT [FK_Authority.GroupPolicies_Authority.Groups_Group_Id]
            GO
            ALTER TABLE [Authority].[GroupPolicies]  WITH CHECK ADD  CONSTRAINT [FK_Authority.GroupPolicies_Authority.Policies_Policy_Id] FOREIGN KEY([Policy_Id])
            REFERENCES [Authority].[Policies] ([Id])
            ON DELETE CASCADE
            GO
            ALTER TABLE [Authority].[GroupPolicies] CHECK CONSTRAINT [FK_Authority.GroupPolicies_Authority.Policies_Policy_Id]
            GO
            ALTER TABLE [Authority].[PolicyAuthorityClaims]  WITH CHECK ADD  CONSTRAINT [FK_Authority.PolicyAuthorityClaims_Authority.Claims_AuthorityClaim_Id] FOREIGN KEY([AuthorityClaim_Id])
            REFERENCES [Authority].[Claims] ([Id])
            GO
            ALTER TABLE [Authority].[PolicyAuthorityClaims] CHECK CONSTRAINT [FK_Authority.PolicyAuthorityClaims_Authority.Claims_AuthorityClaim_Id]
            GO
            ALTER TABLE [Authority].[PolicyAuthorityClaims]  WITH CHECK ADD  CONSTRAINT [FK_Authority.PolicyAuthorityClaims_Authority.Policies_Policy_Id] FOREIGN KEY([Policy_Id])
            REFERENCES [Authority].[Policies] ([Id])
            GO
            ALTER TABLE [Authority].[PolicyAuthorityClaims] CHECK CONSTRAINT [FK_Authority.PolicyAuthorityClaims_Authority.Policies_Policy_Id]
            GO
            ALTER TABLE [Authority].[UserGroups]  WITH CHECK ADD  CONSTRAINT [FK_Authority.UserGroups_Authority.Groups_Group_Id] FOREIGN KEY([Group_Id])
            REFERENCES [Authority].[Groups] ([Id])
            ON DELETE CASCADE
            GO
            ALTER TABLE [Authority].[UserGroups] CHECK CONSTRAINT [FK_Authority.UserGroups_Authority.Groups_Group_Id]
            GO
            ALTER TABLE [Authority].[UserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserGroups_Authority.Users_User_DomainId_User_Email] FOREIGN KEY([User_DomainId], [User_Email])
            REFERENCES [Authority].[Users] ([DomainId], [Email])
            ON DELETE CASCADE
            GO
            ALTER TABLE [Authority].[UserGroups] CHECK CONSTRAINT [FK_Authority.UserGroups_Authority.Users_User_DomainId_User_Email]
            GO
            ALTER TABLE [Authority].[UserPolicies]  WITH CHECK ADD  CONSTRAINT [FK_Authority.UserPolicies_Authority.Policies_Policy_Id] FOREIGN KEY([Policy_Id])
            REFERENCES [Authority].[Policies] ([Id])
            ON DELETE CASCADE
            GO
            ALTER TABLE [Authority].[UserPolicies] CHECK CONSTRAINT [FK_Authority.UserPolicies_Authority.Policies_Policy_Id]
            GO
            ALTER TABLE [Authority].[UserPolicies]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserPolicies_Authority.Users_User_DomainId_User_Email] FOREIGN KEY([User_DomainId], [User_Email])
            REFERENCES [Authority].[Users] ([DomainId], [Email])
            ON DELETE CASCADE
            GO
            ALTER TABLE [Authority].[UserPolicies] CHECK CONSTRAINT [FK_Authority.UserPolicies_Authority.Users_User_DomainId_User_Email]
            GO
            ";
    }
}
