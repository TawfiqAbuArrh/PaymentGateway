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

Insert into [dbo].[Users] (UserName, Password, UserTypeID) values ('Admin', '1234567890', 1);

select a.ID, a.Name, a.Password, b.Name "UserType", a.CreditBalance from [dbo].[Users] a inner join [dbo].[UsersType] b on a.UserTypeID = b.UserType_ID;

Alter Table [dbo].[Transaction] Add UserId int not null;

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

USE PaymentGateway;   
GO  
EXEC sp_rename 'Users', 'IndividualUsers'; 

alter table [dbo].[Users] Add 
    [CreditBalance] DECIMAL (19, 4) DEFAULT ((0)) NOT NULL;

CREATE TABLE [dbo].[BusinessProfile] (
    [UserID]	INT,
    [BusinessTypeID] INT            NOT NULL,
    [ContactName]    NVARCHAR (50)  NOT NULL,
    [ContactPhone]   NVARCHAR (15)  NOT NULL,
    [PDF]            VARCHAR (MAX)  NULL,
    [PDF_Name]       NVARCHAR (255) NULL,
    CONSTRAINT [FK_Business_UsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([ID]),
    CONSTRAINT [FK_Business_BusinessID] FOREIGN KEY ([BusinessTypeID]) REFERENCES [dbo].[BusinessType] ([Business_Id])
);

Alter table [dbo].[Transaction] Add 
  CONSTRAINT [PK_Transaction_ID] PRIMARY KEY ([Transaction_ID]);

--[Transaction_ID]     INT             IDENTITY (1, 1) NOT NULL