CREATE TABLE [dbo].[BusinessProfile] (
    [UserID]         INT            NOT NULL,
    [BusinessTypeID] INT            NOT NULL,
    [ContactName]    NVARCHAR (50)  NOT NULL,
    [ContactPhone]   NVARCHAR (15)  NOT NULL,
    [PDF]            VARCHAR (MAX)  NULL,
    [PDF_Name]       NVARCHAR (255) NULL,
    CONSTRAINT [PK_BusninessProfile_UserID] PRIMARY KEY CLUSTERED ([UserID] ASC),
    CONSTRAINT [FK_Business_UsersID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([ID]),
    CONSTRAINT [FK_Business_BusinessTypeID] FOREIGN KEY ([BusinessTypeID]) REFERENCES [dbo].[BusinessType] ([Business_Id])
);

CREATE TABLE [dbo].[BusinessType] (
    [Business_Id]  INT           IDENTITY (1, 1) NOT NULL,
    [BusinessName] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Business_Id] ASC)
);

CREATE TABLE [dbo].[LoginTokens] (
    [Token]           VARCHAR (255) NOT NULL,
    [UserID]          INT           NULL,
    [TokenTime]       DATETIME      DEFAULT (getdate()) NOT NULL,
    [TokenExpiration] DATETIME      NULL,
    CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED ([Token] ASC),
    CONSTRAINT [FK_User_ID] FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([ID])
);

CREATE TABLE [dbo].[Transaction] (
    [TransactionName]    NVARCHAR (50)   NOT NULL,
    [TransactionAmount]  DECIMAL (19, 4) DEFAULT ((0)) NOT NULL,
    [TransactionType_ID] INT             NOT NULL,
    [UserId]             INT             NOT NULL,
    [Transaction_ID]     VARCHAR (255)   NOT NULL,
    [TransactionTime]    DATETIME        DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_Transaction_ID] PRIMARY KEY CLUSTERED ([Transaction_ID] ASC),
    CONSTRAINT [FK_Transaction_UserID] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([ID]),
    CONSTRAINT [FK_TransactionType_ID] FOREIGN KEY ([TransactionType_ID]) REFERENCES [dbo].[TransactionTypes] ([TransactionType_ID])
);

CREATE TABLE [dbo].[TransactionTypes] (
    [TransactionType_ID]   INT           IDENTITY (1, 1) NOT NULL,
    [TransactionType_Name] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TransactionType_ID] PRIMARY KEY CLUSTERED ([TransactionType_ID] ASC)
);

CREATE TABLE [dbo].[Users] (
    [UserName]      NVARCHAR (50)   NOT NULL,
    [Password]      NVARCHAR (100)  NOT NULL,
    [UserTypeID]    INT             NOT NULL,
    [ID]            INT             IDENTITY (1, 1) NOT NULL,
    [AdminApproval] BIT             DEFAULT ((0)) NOT NULL,
    [CreditBalance] DECIMAL (19, 4) DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_UsersType] FOREIGN KEY ([UserTypeID]) REFERENCES [dbo].[UsersType] ([UserType_ID])
);

CREATE TABLE [dbo].[UsersType] (
    [UserType_ID] INT           NOT NULL,
    [Name]        NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_UsersType] PRIMARY KEY CLUSTERED ([UserType_ID] ASC)
);

Insert into [dbo].[Users] (UserName, Password, UserTypeID) values ('Admin', '12345', 1);