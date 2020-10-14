DECLARE @Id INT
DECLARE @Id1 INT 
DECLARE @Id2 INT 
DECLARE @OrderId INT 

INSERT INTO [Inventory].[Supplier]
(
  [Name],
  [Phone]
)
VALUES
(
  'JB HIFI',
  '1234'
)
SELECT @Id=@@IDENTITY 

INSERT INTO [Inventory].[Item]
(
  [Name],
  [Price],
  [DeliveryFee],
  [SupplierId]
)
VALUES
(
  'Playstation 5',
  '500',
  '10',
  @Id
)
SELECT @Id1=@@IDENTITY 

INSERT INTO [Inventory].[Item]
(
  [Name],
  [Price],
  [DeliveryFee],
  [SupplierId]
)
VALUES
(
  'XBox one',
  '400',
  '20',
  @Id
)

INSERT INTO [Inventory].[Supplier]
(
  [Name],
  [Phone]
)
VALUES
(
  'Harvey Norman',
  '5678'
)
SELECT @Id=@@IDENTITY 

INSERT INTO [Inventory].[Item]
(
  [Name],
  [Price],
  [DeliveryFee],
  [SupplierId]
)
VALUES
(
  'Canon EOS 5D Mark IV',
  '4000',
  '30',
  @Id
)
SELECT @Id2=@@IDENTITY 

INSERT INTO [Inventory].[Item]
(
  [Name],
  [Price],
  [DeliveryFee],
  [SupplierId]
)
VALUES
(
  'Canon EOS 6D Mark II',
  '2000',
  '35',
  @Id
)

INSERT INTO [Orders].[Order]
(
  [Priority],
  [DeliveryFee]
)
VALUES
(
  0,
  10
)
SELECT @OrderId=@@IDENTITY 

INSERT INTO [Orders].[OrderItem]
(
  [OrderId],
  [LineNumber],
  [ItemId],
  [Price]
)
VALUES
(
   @OrderId,
   1,
   @Id1,
   '499'
)

INSERT INTO [Orders].[OrderItem]
(
  [OrderId],
  [LineNumber],
  [ItemId],
  [Price]
)
VALUES
(
   @OrderId,
   2,
   @Id2,
  '3900'
)
GO