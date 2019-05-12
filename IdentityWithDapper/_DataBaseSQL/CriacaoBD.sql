USE master
IF EXISTS(select * from sys.databases where name='IdentityWithDapper')
DROP DATABASE IdentityWithDapper

GO

CREATE DATABASE IdentityWithDapper;

GO

--ApplicationUser
DROP TABLE IF EXISTS [IdentityWithDapper].[dbo].[ApplicationUser]

GO

CREATE TABLE [IdentityWithDapper].[dbo].[ApplicationUser](
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserName] NVARCHAR(256) NOT NULL,
	[NormalizedUserName] NVARCHAR(256) NOT NULL,
	[Email] NVARCHAR(256) NULL,
	[NormalizedEmail] NVARCHAR(256) NULL,
	[EmailConfirmed] BIT NOT NULL,
	[PasswordHash] NVARCHAR(MAX) NULL,
	[PhoneNumber] NVARCHAR(50) NULL,
	[PhoneNumberConfirmed]BIT NOT NULL,
	[TwoFactorEnabled] BIT NOT NULL
)

GO

CREATE INDEX [IX_ApplicationUser_NormalizedUserName] ON [IdentityWithDapper].[dbo].[ApplicationUser] ([NormalizedUserName])

GO

CREATE INDEX [IX_ApplicationUser_NormlizedEmail] ON [IdentityWithDapper].[dbo].[ApplicationUser] ([NormalizedEmail])

GO

--ApplicationRole
DROP TABLE IF EXISTS IdentityWithDapper.dbo.ApplicationRole; 

GO

CREATE TABLE [IdentityWithDapper].[dbo].[ApplicationRole](
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(256) NOT NULL,
	[NormalizedName] NVARCHAR(256) NOT NULL
)

GO

CREATE INDEX [IX_ApplicationRole_NormalizedName] ON [IdentityWithDapper].[dbo].[ApplicationRole] ([NormalizedName])

GO

--ApplicationUserRole
DROP TABLE IF EXISTS IdentityWithDapper.dbo.ApplicationUserRole; 

GO

CREATE TABLE [IdentityWithDapper].[dbo].[ApplicationUserRole](
	[UserId] INT NOT NULL,
	[RoleId] INT NOT NULL,
	PRIMARY KEY ([UserId], [RoleId]),
	CONSTRAINT [FK_ApplicationUserRole_User] FOREIGN KEY ([UserId]) REFERENCES [IdentityWithDapper].[dbo].[ApplicationUser](Id),
	CONSTRAINT [FK_ApplicationUserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [IdentityWithDapper].[dbo].[ApplicationRole](Id)
)