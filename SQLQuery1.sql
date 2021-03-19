USE [PaymentGateway]
GO

/****** Object: Table [dbo].[Admin] Script Date: 18/3/2021 8:48:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Admin] (
    [Id]        INT           NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [Passwords] NVARCHAR (50) NOT NULL
);


