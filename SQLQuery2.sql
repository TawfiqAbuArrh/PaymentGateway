CREATE TABLE [dbo].[UsersType]
(
	[UserType_ID] INT NOT NULL , 
    [Name] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_UsersType] PRIMARY KEY ([UserType_ID])
);

CREATE TABLE [dbo].[Users]
(
	[ID] INT NOT NULL IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL, 
    [UserTypeID] INT NOT NULL , 
    [CreditBalance] DECIMAL NOT NULL DEFAULT 0.0, 
    CONSTRAINT [FK_UsersType] FOREIGN KEY ([UserTypeID]) REFERENCES [UsersType]([UserType_ID]), 
    CONSTRAINT [PK_Users] PRIMARY KEY ([ID])
);


select a.ID, a.Name, a.Password, b.Name "UserType", a.CreditBalance from [dbo].[Users] a inner join [dbo].[UsersType] b on a.UserTypeID = b.UserType_ID;

Alter Table [dbo].[Users] Add CreditBalance decimal(19,4) NOT NULL DEFAULT 0;

CREATE TABLE [dbo].[LoginTokens]
(
	[Token] varchar(255) NOT NULL, 
    [UserID] int,
    [TokenTime] datetime not null default GETDATE(),
    [TokenExpiration] datetime,
    CONSTRAINT [PK_Token] PRIMARY KEY ([Token]),
    CONSTRAINT [FK_User_ID] FOREIGN  KEY ([UserID]) 
    REFERENCES [Users](ID)
);


EXEC sp_rename 'TransactionTypes.TransactionName', 'TransactionType_Name', 'COLUMN';