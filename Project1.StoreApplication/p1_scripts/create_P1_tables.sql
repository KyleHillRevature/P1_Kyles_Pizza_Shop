CREATE DATABASE Kyles_Pizza_Shop;

CREATE TABLE Products(
Id INT PRIMARY KEY IDENTITY,
Name1 varchar(50) NOT NULL,
Description1 varchar(100) NOT NULL,
ProductPrice decimal(19,4) NOT NULL);

CREATE TABLE Customers(
Id INT PRIMARY KEY IDENTITY,
FirstName varchar(50) NOT NULL,
LastName varchar(50) NOT NULL);


CREATE TABLE OrderItems(
Id uniqueidentifier NOT NULL DEFAULT newid() PRIMARY KEY,
OrderId uniqueidentifier NOT NULL foreign key references Orders(Id) on delete cascade,
ProductId INT NOT NULL FOREIGN KEY REFERENCES Products(Id) ON DELETE CASCADE);

create table Orders(
Id uniqueidentifier primary key not null,
OrderDate datetime2 not null,
CustomerId INT NOT NULL FOREIGN KEY REFERENCES Customers(Id) ON DELETE CASCADE,
LocationId int not null foreign key references Locations(Id) on delete cascade,
TotalPrice decimal(19,4) not null
);

create table Locations(
Id int primary key IDENTITY,
CityName varchar(50) not null
);

create table LocationInventory(
Id int primary key identity,
LocationId int not null foreign key references Locations(Id) on delete cascade,
ProductId int not null foreign key references Products(Id) on delete cascade,
Stock int not null 
);
