IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = N'Inventory')
  EXEC('CREATE SCHEMA [Inventory]')
GO

CREATE TABLE [Inventory].[Supplier] 
(
  [Id] INT IDENTITY,
  [ExternalId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
  [Name] NVARCHAR(255) NOT NULL,
  [Phone] NVARCHAR(20) NOT NULL
  CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_Supplier_ExternalId] ON 
  [Inventory].[Supplier]
(
  [ExternalId] ASC
)
INCLUDE ([Id]) 
GO

CREATE TABLE [Inventory].[Item]
(
  [Id] INT IDENTITY,
  [ExternalId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
  [Name] NVARCHAR(255) NOT NULL,
  [Price] DECIMAL(18, 2) NOT NULL,
  [DeliveryFee] DECIMAL(8, 2) NOT NULL,
  [SupplierId] INT NOT NULL,
  CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([Id] ASC),
  CONSTRAINT [FK_Item_Supplier] FOREIGN KEY ([SupplierId]) REFERENCES [Inventory].[Supplier] ([Id]) ON DELETE CASCADE,
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_Item_ExternalId] ON
  [Inventory].[Item]
(
  [ExternalId] ASC
)
INCLUDE ([Id]) 
GO


IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = N'Orders')
  EXEC('CREATE SCHEMA [Orders]')
GO

CREATE TABLE [Orders].[Order] 
(
  [Id] INT IDENTITY,
  [ExternalId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
  [Priority] INT NOT NULL,
  [DeliveryFee] DECIMAL(10, 2) NOT NULL,
  CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [UX_Oder_ExternalId] ON 
  [Orders].[Order] 
(
  [ExternalId] ASC
)
INCLUDE ([Id]) 
GO

CREATE TABLE [Orders].[OrderItem] 
(
  [OrderId] INT NOT NULL,
  [LineNumber] INT NOT NULL,
  [ItemId] INT NOT NULL,
  [Price] DECIMAL(8, 2) NOT NULL
  CONSTRAINT [PK_OrderItem] PRIMARY KEY ([OrderId], [LineNumber]),
  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY ([OrderId]) REFERENCES [Orders].[Order] ([Id]),
  CONSTRAINT [FK_OrderItem_ItemId] FOREIGN KEY ([OrderId]) REFERENCES [Inventory].[Item] ([Id]),
)
GO

CREATE VIEW [Orders].[Item]
AS
SELECT
  [Id],
  [ExternalId],
  [Name],
  [DeliveryFee],
  [Price]
FROM
  [Inventory].[Item]
GO
