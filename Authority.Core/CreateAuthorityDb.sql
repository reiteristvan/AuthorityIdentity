
set ansi_nulls on
set quoted_identifier on
go

-- drop tables
if OBJECT_ID(N'Authority.UserPolicies', N'U') is not null
begin
	drop table [Authority].[UserPolicies]
end

if OBJECT_ID(N'Authority.UserGroups', N'U') is not null
begin
	drop table [Authority].[UserGroups]
end

if OBJECT_ID(N'Authority.GroupPolicies', N'U') is not null
begin
	drop table [Authority].[GroupPolicies]
end

if OBJECT_ID(N'Authority.PolicyClaims', N'U') is not null
begin
	drop table [Authority].[PolicyClaims]
end

if OBJECT_ID(N'Authority.Invites', N'U') is not null
begin
	drop table [Authority].[Invites]
end

if OBJECT_ID(N'Authority.Users', N'U') is not null
begin
	drop table [Authority].[Users]
end

if OBJECT_ID(N'Authority.Claims', N'U') is not null
begin
	drop table [Authority].[Claims]
end

if OBJECT_ID(N'Authority.Policies', N'U') is not null
begin
	drop table [Authority].[Policies]
end

if OBJECT_ID(N'Authority.Groups', N'U') is not null
begin
	drop table [Authority].[Groups]
end

if OBJECT_ID(N'Authority.Domains', N'U') is not null
begin
	drop table [Authority].[Domains]
end

-- drop schema
if exists (select schema_name from INFORMATION_SCHEMA.SCHEMATA where SCHEMA_NAME = 'Authority')
begin
	drop schema Authority
end

-- create schema
exec sp_executesql N'create schema Authority'
go

-- create tables
create table [Authority].[Domains]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[IsActive] [bit] NOT NULL,
	constraint [PK_Authority.Domains] primary key clustered
	(
		[Id] asc
	)
	with
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary]
)
go

create table [Authority].[Users]
(
	[DomainId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
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
	constraint [PK_Authority.Users] primary key clustered
	(
		[DomainId] asc,
		[Email] asc
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_Users_Domains] foreign key ([DomainId])
		references [Authority].[Domains] ([Id])
		on delete cascade
)
go

create table [Authority].[Policies]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Default] [bit] NOT NULL,
	[DomainId] [uniqueidentifier] NOT NULL,
	constraint [PK_Authority.Policies] primary key clustered
	(
		[Id] asc
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_Policies_Domains] foreign key ([DomainId])
		references [Authority].[Domains] ([Id])
		on delete cascade
)
go

create table [Authority].[Claims]
(
	[Id] [uniqueidentifier] NOT NULL,
	[FriendlyName] [nvarchar](128) NOT NULL,
	[Issuer] [nvarchar](256) NOT NULL,
	[Type] [nvarchar](256) NOT NULL,
	[Value] [nvarchar](512) NOT NULL,
	[DomainId] [uniqueidentifier] NULL,
	constraint [PK_Authority.Claims] primary key clustered
	(
		[Id] asc
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_Claims_Domains] foreign key ([DomainId])
		references [Authority].[Domains] ([Id])
		on delete cascade
)
go

create table [Authority].[Groups]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Default] [bit] NOT NULL,
	[DomainId] [uniqueidentifier] NOT NULL,
	constraint [PK_Authority.Groups] primary key clustered
	(
		[Id] asc
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_Groups_Domains] foreign key ([DomainId])
		references [Authority].[Domains] ([Id])
		on delete cascade
)
go

create table [Authority].[Invites]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Created] [datetimeoffset](7) NOT NULL,
	[Accepted] [bit] NOT NULL,
	[Expire] [datetimeoffset](7) NULL,
	[DomainId] [uniqueidentifier] NOT NULL,
	constraint [PK_Authority.Invites] primary key clustered
	(
		[Id] asc
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_Invites_Domains] foreign key ([DomainId])
		references [Authority].[Domains] ([Id])
		on delete cascade
)
go

create table [Authority].[UserPolicies]
(
	[User_DomainId] [uniqueidentifier] NOT NULL,
	[User_Email] [nvarchar](256) NOT NULL,
	[Policy_Id] [uniqueidentifier] NOT NULL,
	constraint [PK_Authority.UserPolicies] primary key clustered
	(
		[User_DomainId] ASC,
		[User_Email] ASC,
		[Policy_Id] ASC
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_UserPolicies_Policy_Id] foreign key (Policy_Id)
		references [Authority].[Policies] ([Id])
		on delete no action,
	constraint [FK_UserPolicies_User] foreign key ([User_DomainId], [User_Email])
		references [Authority].[Users] ([DomainId], [Email])
		on delete no action
)

alter table [Authority].[UserPolicies] check constraint [FK_UserPolicies_Policy_Id]
alter table [Authority].[UserPolicies] check constraint [FK_UserPolicies_User]
go

create table [Authority].[UserGroups]
(
	[User_DomainId] [uniqueidentifier] NOT NULL,
	[User_Email] [nvarchar](256) NOT NULL,
	[Group_Id] [uniqueidentifier] NOT NULL,
	constraint [PK_Authority.UserGroups] primary key clustered
	(
		[User_DomainId] ASC,
		[User_Email] ASC,
		[Group_Id] ASC
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_UserGroups_Group_Id] foreign key (Group_Id)
		references [Authority].[Groups] ([Id])
		on delete no action,
	constraint [FK_UserGroups_User] foreign key ([User_DomainId], [User_Email])
		references [Authority].[Users] ([DomainId], [Email])
		on delete no action
)

alter table [Authority].[UserGroups] check constraint [FK_UserGroups_Group_Id]
alter table [Authority].[UserGroups] check constraint [FK_UserGroups_User]
go

create table [Authority].[GroupPolicies]
(
	[Group_Id] [uniqueidentifier] NOT NULL,
	[Policy_Id] [uniqueidentifier] NOT NULL,
	constraint [PK_Authority.GroupPolicies] primary key clustered
	(
	    [Group_Id] ASC,
	    [Policy_Id] ASC
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_GroupPolicies_Group_Id] foreign key (Group_Id)
		references [Authority].[Groups] ([Id])
		on delete no action,
	constraint [FK_GroupPolicies_Policy_Id] foreign key ([Policy_Id])
		references [Authority].[Policies] ([Id])
		on delete no action
)

alter table [Authority].[GroupPolicies] check constraint [FK_GroupPolicies_Group_Id]
alter table [Authority].[GroupPolicies] check constraint [FK_GroupPolicies_Policy_Id]
go

create table [Authority].[PolicyClaims]
(
	[Policy_Id] [uniqueidentifier] NOT NULL,
	[Claim_Id] [uniqueidentifier] NOT NULL,
	constraint [PK_Authority.PolicyClaims] primary key clustered
	(
	    [Policy_Id] ASC,
		[Claim_Id] ASC
	)
	with 
	(
		PAD_INDEX = OFF, 
		STATISTICS_NORECOMPUTE = OFF, 
		IGNORE_DUP_KEY = ON, 
		ALLOW_ROW_LOCKS = ON, 
		ALLOW_PAGE_LOCKS = ON
	) on [Primary],
	constraint [FK_PolicyClaims_Policy_Id] foreign key (Policy_Id)
		references [Authority].[Policies] ([Id])
		on delete no action,
	constraint [FK_GroupPolicies_Claim_Id] foreign key ([Claim_Id])
		references [Authority].[Claims] ([Id])
		on delete no action
)

alter table [Authority].[PolicyClaims] check constraint [FK_PolicyClaims_Policy_Id]
alter table [Authority].[PolicyClaims] check constraint [FK_GroupPolicies_Claim_Id]
go