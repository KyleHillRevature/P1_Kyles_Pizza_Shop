insert into Locations values ('Amherst'),('Elyria')

declare @locationId1 as int
select @locationId1 = Id
from Locations 
where CityName = 'Amherst'

declare @locationId2 as int
select @locationId2 = Id
from Locations 
where CityName = 'Elyria'

insert into Products values ('Calzone','Cheese and tomato',6),('Cheese pizza','Extra cheese for no cost!',11),
('Wings','Nice and greasy',8),('Pineapple pizza','Very polarizing',10)


declare @productId as int
select @productId = Id
from Products 
where Name1 = 'Calzone'

declare @productId1 as int
select @productId1 = Id
from Products 
where Name1 = 'Cheese pizza'

declare @productId2 as int
select @productId2 = Id
from Products 
where Name1 = 'Wings'

declare @productId3 as int
select @productId3 = Id
from Products 
where Name1 = 'Pineapple pizza'

insert into LocationInventory values (@locationId1,@productId,10),(@locationId1,@productId1,10),(@locationId1,@productId2,10),
(@locationId1,@productId3,0),(@locationId2,@productId,10),(@locationId2,@productId1,10),(@locationId2,@productId2,10),
(@locationId2,@productId3,0)

insert into Customers values ('Shakira','Garfunkel')

delete from Customers
delete from Locations
delete from LocationInventory
delete from Products
delete from Orders
delete from OrderItems

select * from LocationInventory
select * from Locations
select * from Products
select * from Customers
select * from OrderItems
select * from Orders