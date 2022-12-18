/*
CREATE TABLE Bills(
Id int primary key IDENTITY(1,1),
BillNo int Not null ,
CustomerName varchar(Max) Not null,
CustomerPhoneNumber varchar(max) Not null,
RecivedDate date not null,
DeliveryDate date not null,
Amount Decimal,
TotalQuantity int,
UserId int Not null
)

CREATE TABLE Items(
Id int primary key IDENTITY(1,1),
BillNo int Not null ,
ItemQuantity int,
ItemName varchar(max),
ItemPrice decimal,
UserId int Not null
)



create Table ShopkeeperUsers(
Id int primary key identity(1,1),
[Name] varChar(max),
[Number] varChar(max),
Email varchar(max),
[Address] varchar(Max),
[Password] varchar(max),
CurrentBillNo int,
GstNo varchar(Max)
)

create table Inventory(
Id int primary key identity(1,1),
UserId int ,
[Name] varchar(max),
Price decimal,
)


*/