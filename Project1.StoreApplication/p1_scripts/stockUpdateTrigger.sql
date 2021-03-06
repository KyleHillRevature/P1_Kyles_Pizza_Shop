create trigger UpdateStock
on OrderItems
after insert
as
begin
declare @productID int
declare @locationID int
select @productID = ProductId, @locationID = LocationId
from inserted join Orders
on inserted.OrderId = Orders.Id

update LocationInventory
set Stock = Stock -1
where ProductId = @productID and LocationId = @locationID 

end

